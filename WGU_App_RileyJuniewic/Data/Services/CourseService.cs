using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ICourseService
{
    public Task<List<Course>> GetAllCoursesAsync();
    public Task<Result<Course>> GetCourseAsync(Guid courseId);
    public Task<Result<Course>> CreateCourseAsync(CreateCourseRequest request);
    public Task<Result<Course>> UpdateCourseAsync(UpdateCourseRequest request);
    public Task DeleteCourseAsync(Guid courseId);   
}

public class CourseService(SqlDataAccessAsync sqlDataAccess) :
    ICourseService,
    ICreateService<CreateCourseRequest, Course>,
    IModifyService<UpdateCourseRequest, Course>
{
    public async Task<Result<Course>> CreateAsync(CreateCourseRequest request) => await CreateCourseAsync(request);

    public async Task<Result<Course>> CreateCourseAsync(CreateCourseRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var type = Enum.TryParse(request.Status, out CourseStatus statusEnum);
        if (!type) return Result.Error("Invalid course status");

        var course = Course.CreateNewInstance(request.TermId, request.InstructorId, request.Title, statusEnum, request.StartDate, request.EndDate);
        var result = await ValidateCourseAsync(course);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().InsertAsync(course);
        return course;
    }

    public async Task DeleteAsync(Guid id) => await DeleteCourseAsync(id);
    public async Task DeleteCourseAsync(Guid courseId)
    {
        // Delete Assesements & Notes
        await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == courseId).DeleteAsync();
        await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.CourseId == courseId).DeleteAsync();

        // Delete Courses
        await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == courseId).DeleteAsync();
    }

    public Task<List<Course>> GetAllCoursesAsync() => sqlDataAccess.GetConnection().Table<Course>().ToListAsync();

    public async Task<Result<Course>> GetCourseAsync(Guid courseId)
    {
        var course = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        if (course == null)
            return Result.Error("Course not found");
            
        return course;
    }

    public async Task<Result<Course>> UpdateAsync(UpdateCourseRequest request) => await UpdateCourseAsync(request);

    public async Task<Result<Course>> UpdateCourseAsync(UpdateCourseRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var type = Enum.TryParse(request.Status, out CourseStatus statusEnum);
        if (!type) return Result.Error("Invalid course status");
            
        var course = Course.CreateInstance(request.CourseId, request.TermId, request.InstructorId, request.Title, statusEnum, request.StartDate, request.EndDate);
        
        var result = await ValidateCourseAsync(course);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().UpdateAsync(course);
        return course;
    }
    
    internal async Task<Result> ValidateCourseAsync(Course course)
    {
        var validInstructor = await sqlDataAccess.GetConnection().Table<Instructor>().Where(x => x.InstructorId == course.InstructorId).FirstOrDefaultAsync();
        if (validInstructor == null)
            return Result.Error("Instructor does not exist");

        var validTerm = await sqlDataAccess.GetConnection().Table<Term>().Where(x => x.TermId == course.TermId).FirstOrDefaultAsync();
        if (validTerm == null)
            return Result.Error("Term does not exist");

        var otherCourses = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.TermId == course.TermId && x.CourseId != course.CourseId).ToListAsync();
        foreach (var otherCourse in otherCourses)
        {
            // if (otherCourse.StartDate <= course.EndDate && otherCourse.EndDate >= course.StartDate)
            //     return Result.Error("Course overlaps with an existing course");
            if (otherCourse.Title == course.Title)
                return Result.Error("Course title already exists");
        }

        return Result.Success();
    }
}