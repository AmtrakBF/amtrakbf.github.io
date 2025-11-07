using Ardalis.Result;
using Plugin.LocalNotification;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IAssessmentService
{
    Task<IEnumerable<Assessment>> GetAllAssessmentsAsync();
    Task<Result<Assessment>> GetAssessmentAsync(Guid id);
    Task<Result<Assessment>> CreateAssessmentAsync(CreateAssessmentRequest request);
    Task<Result<Assessment>> UpdateAssessmentAsync(UpdateAssessmentRequest request);
    Task<Result> DeleteAssessmentAsync(Guid id);
    public Task<Result> SetNotificationAsync(Guid assessmentId, int startId, int endId);
    public Task<Result> RemoveNotificationAsync(Guid assessmentId);
}

public class AssessmentService(SqlDataAccessAsync sqlDataAccess) :
    IAssessmentService,
    ICreateService<CreateAssessmentRequest, Assessment>,
    IModifyService<UpdateAssessmentRequest, Assessment>
{
    public async Task<Result<Assessment>> CreateAssessmentAsync(CreateAssessmentRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var type = Enum.TryParse(request.Type, out AssessmentType typeEnum);
        if (!type) return Result.Error("Invalid assessment type");

        var assessment = Assessment.CreateNewInstance(request.CourseId, request.Name, typeEnum, request.StartDate, request.EndDate);
        var result = await ValidateAssessmentAsync(assessment);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.InsertAsync(assessment);
        return assessment;
    }

    public async Task<Result<Assessment>> CreateAsync(CreateAssessmentRequest request) => await CreateAssessmentAsync(request);

    public async Task<Result> DeleteAssessmentAsync(Guid id)
    {
        var assessment = await GetAssessmentAsync(id);
        if (!assessment.IsError())
        {
            LocalNotificationCenter.Current.Cancel(assessment.Value.NotificationStartId);
            LocalNotificationCenter.Current.Cancel(assessment.Value.NotificationEndId);
        }
        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.Table<Assessment>().Where(x => x.AssessmentId == id).DeleteAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id) => await DeleteAssessmentAsync(id);

    public async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync()
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        return await connection.Table<Assessment>().ToListAsync();
    }

    public async Task<Result<Assessment>> GetAssessmentAsync(Guid id)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var assessment = await connection.Table<Assessment>().Where(x => x.AssessmentId == id).FirstOrDefaultAsync();
        if (assessment == null)
            return Result.Error("Assessment not found");
        return new(assessment);
    }

    public async Task<Result<Assessment>> UpdateAssessmentAsync(UpdateAssessmentRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var type = Enum.TryParse(request.Type, out AssessmentType typeEnum);
        if (!type) return Result.Error("Invalid assessment type");

        var assessment = Assessment.CreateInstance(request.Id, request.CourseId, request.Name, typeEnum, request.StartDate, request.EndDate);
        var result = await ValidateAssessmentAsync(assessment);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(assessment);
        return assessment;
    }

    public async Task<Result<Assessment>> UpdateAsync(UpdateAssessmentRequest request) => await UpdateAssessmentAsync(request);

    public async Task<Result> ValidateAssessmentAsync(Assessment assessment)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var existingCourse = await connection.Table<Course>().Where(x => x.CourseId == assessment.CourseId).FirstOrDefaultAsync();
        if (existingCourse == null)
            return Result.Error("Course does not exist");

        var exisitingName =await connection.Table<Assessment>().Where(x => x.CourseId == assessment.CourseId && x.Name == assessment.Name).FirstOrDefaultAsync();
        if (exisitingName != null && exisitingName.AssessmentId != assessment.AssessmentId)
            return Result.Error("Assessment name already exists");

        var overlappingAssessments = await connection.Table<Assessment>().Where(x => x.CourseId == assessment.CourseId && x.StartDate <= assessment.EndDate && x.EndDate >= assessment.StartDate).ToListAsync();
        if (overlappingAssessments.Any(x => x.AssessmentId != assessment.AssessmentId))
            return Result.Error("Assessment overlaps with existing assessment");

        return Result.Success();
    }

    public async Task<Result> RemoveNotificationAsync(Guid assessmentId)
    {
        var assessment = await GetAssessmentAsync(assessmentId);
        if (assessment.IsError())
            return Result.Error("Course not found");

        LocalNotificationCenter.Current.Cancel(assessment.Value.NotificationStartId);
        LocalNotificationCenter.Current.Cancel(assessment.Value.NotificationEndId);

        assessment.Value.NotificationStartId = -1;
        assessment.Value.NotificationEndId = -1;

       var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(assessment.Value);

        return Result.Success();
    }

    public async Task<Result> SetNotificationAsync(Guid assessmentId, int startId, int endId)
    {
        var assessment = await GetAssessmentAsync(assessmentId);
        if (assessment.IsError())
            return Result.Error("Course not found");

        //! Delete old notifications
        LocalNotificationCenter.Current.Cancel(assessment.Value.NotificationStartId);
        LocalNotificationCenter.Current.Cancel(assessment.Value.NotificationEndId);

        assessment.Value.NotificationStartId = startId;
        assessment.Value.NotificationEndId = endId;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(assessment.Value);
        return Result.Success();
    }
}