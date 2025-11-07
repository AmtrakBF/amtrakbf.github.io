using Ardalis.Result;
using Plugin.LocalNotification;
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
    public Task<Result> DeleteCourseAsync(Guid courseId);   
    public Task<Result> SetNotificationAsync(Guid courseId, int startId, int endId);
    public Task<Result> RemoveNotificationAsync(Guid courseId);
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

        var course = Course.CreateNewInstance(request.TermId, request.InstructorId, request.Title,
                        statusEnum, request.StartDate, request.EndDate, request.Notes);
        var result = await ValidateCourseAsync(course);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.InsertAsync(course);
        return course;
    }

    public async Task<Result> DeleteAsync(Guid id) => await DeleteCourseAsync(id);
    public async Task<Result> DeleteCourseAsync(Guid courseId)
    {
        var course = await GetCourseAsync(courseId);
        if (!course.IsError())
        {
            LocalNotificationCenter.Current.Cancel(course.Value.NotificationStartId);
            LocalNotificationCenter.Current.Cancel(course.Value.NotificationEndId);
        }

        // Delete Assesements & Notes
        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.Table<Assessment>().Where(x => x.CourseId == courseId).DeleteAsync();
        await connection.Table<Note>().Where(x => x.CourseId == courseId).DeleteAsync();

        // Delete Courses
        await connection.Table<Course>().Where(x => x.CourseId == courseId).DeleteAsync();

        return Result.Success();
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        return await connection.Table<Course>().ToListAsync();
    }

    public async Task<Result<Course>> GetCourseAsync(Guid courseId)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var course = await connection.Table<Course>().Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        if (course == null)
            return Result.Error("Course not found");
            
        return course;
    }

    public async Task<Result> RemoveNotificationAsync(Guid courseId)
    {
        var course = await GetCourseAsync(courseId);
        if (course.IsError())
            return Result.Error("Course not found");

        //! Delete old notifications
        LocalNotificationCenter.Current.Cancel(course.Value.NotificationStartId);
        LocalNotificationCenter.Current.Cancel(course.Value.NotificationEndId);

        course.Value.NotificationStartId = -1;
        course.Value.NotificationEndId = -1;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(course.Value);

        return Result.Success();
    }

    public async Task<Result> SetNotificationAsync(Guid courseId, int startId, int endId)
    {
        var course = await GetCourseAsync(courseId);
        if (course.IsError())
            return Result.Error("Course not found");

        //! Delete old notifications
        LocalNotificationCenter.Current.Cancel(course.Value.NotificationStartId);
        LocalNotificationCenter.Current.Cancel(course.Value.NotificationEndId);

        course.Value.NotificationStartId = startId;
        course.Value.NotificationEndId = endId;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(course.Value);
        return Result.Success();
    }

    public async Task<Result<Course>> UpdateAsync(UpdateCourseRequest request) => await UpdateCourseAsync(request);

    public async Task<Result<Course>> UpdateCourseAsync(UpdateCourseRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var type = Enum.TryParse(request.Status, out CourseStatus statusEnum);
        if (!type) return Result.Error("Invalid course status");
            
        var course = Course.CreateInstance(request.Id, request.TermId, request.InstructorId,
                        request.Title, statusEnum, request.StartDate, request.EndDate, request.Notes);
        
        var result = await ValidateCourseAsync(course);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(course);
        return course;
    }
    
    internal async Task<Result> ValidateCourseAsync(Course course)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var validInstructor = await connection.Table<Instructor>().Where(x => x.InstructorId == course.InstructorId).FirstOrDefaultAsync();
        if (validInstructor == null)
            return Result.Error("Instructor does not exist");

        var validTerm = await connection.Table<Term>().Where(x => x.TermId == course.TermId).FirstOrDefaultAsync();
        if (validTerm == null)
            return Result.Error("Term does not exist");

        var otherCourses = await connection.Table<Course>().Where(x => x.TermId == course.TermId && x.CourseId != course.CourseId).ToListAsync();
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