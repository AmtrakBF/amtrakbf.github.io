using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ICourseService
{
    public Task<List<Course>> GetAllCoursesAsync();
    public Task<Course> GetCourseAsync(Guid courseId);
    public Task<Course> CreateCourseAsync(CreateCourseRequest request);
    public Task<Course> UpdateCourseAsync(UpdateCourseRequest request);
    public Task DeleteCourseAsync(Guid courseId);   
}

public class CourseService(SqlDataAccessAsync sqlDataAccess) : ICourseService
{
    public async Task<Course> CreateCourseAsync(CreateCourseRequest request)
    {
        var course = Course.CreateNewInstance(request.TermId, request.InstructorId, request.Title, request.Status, request.StartDate, request.EndDate);
        await ValidateCourseAsync(course);

        await sqlDataAccess.GetConnection().InsertAsync(course);
        return course;
    }

    public async Task DeleteCourseAsync(Guid courseId)
    {
        // Delete Assesements & Notes
        await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == courseId).DeleteAsync();
        await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.CourseId == courseId).DeleteAsync();

        // Delete Courses
        await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == courseId).DeleteAsync();
    }

    public Task<List<Course>> GetAllCoursesAsync() => sqlDataAccess.GetConnection().Table<Course>().ToListAsync();

    public async Task<Course> GetCourseAsync(Guid courseId)
    {
        var course = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        if (course == null)
            throw new UserException("Course not found");
        return course;
    }

    public async Task<Course> UpdateCourseAsync(UpdateCourseRequest request)
    {
        var course = Course.CreateInstance(request.CourseId, request.TermId, request.InstructorId, request.Title, request.Status, request.StartDate, request.EndDate);
        await ValidateCourseAsync(course);

        await sqlDataAccess.GetConnection().UpdateAsync(course);
        return course;
    }
    
    internal async Task<bool> ValidateCourseAsync(Course course)
    {
        var validInstructor = await sqlDataAccess.GetConnection().Table<Instructor>().Where(x => x.InstructorId == course.InstructorId).FirstOrDefaultAsync();
        if (validInstructor == null)
            throw new UserException("Instructor does not exist");

        var validTerm = await sqlDataAccess.GetConnection().Table<Term>().Where(x => x.TermId == course.TermId).FirstOrDefaultAsync();
        if (validTerm == null)
            throw new UserException("Term does not exist");

        var otherCourses = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.TermId == course.TermId && x.CourseId != course.CourseId).ToListAsync();
        foreach (var otherCourse in otherCourses)
        {
            if (otherCourse.StartDate <= course.EndDate && otherCourse.EndDate >= course.StartDate)
                throw new UserException("Course overlaps with an existing course");
        }

        return true;
    }
}