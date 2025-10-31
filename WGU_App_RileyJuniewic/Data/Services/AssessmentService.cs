using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IAssessmentService
{
    Task<IEnumerable<Assessment>> GetAllAssessmentsAsync();
    Task<Result<Assessment>> GetAssessmentAsync(Guid id);
    Task<Result<Assessment>> CreateAssessmentAsync(CreateAssessmentRequest request);
    Task<Result<Assessment>> UpdateAssessmentAsync(UpdateAssessmentRequest request);
    Task DeleteAssessmentAsync(Guid id);
}

public class AssessmentService(SqlDataAccessAsync sqlDataAccess) : IAssessmentService
{
    public async Task<Result<Assessment>> CreateAssessmentAsync(CreateAssessmentRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetAllErrors());

        var assessment = Assessment.CreateNewInstance(request.CourseId, request.Name, request.Type, request.StartDate, request.EndDate);
        var result = await ValidateAssessmentAsync(assessment);
        if (result.IsError())
            return result;
            
        await sqlDataAccess.GetConnection().InsertAsync(assessment);
        return assessment;
    }

    public async Task DeleteAssessmentAsync(Guid id) =>
        await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.AssessmentId == id).DeleteAsync();

    public async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync() => await sqlDataAccess.GetConnection().Table<Assessment>().ToListAsync();

    public async Task<Result<Assessment>> GetAssessmentAsync(Guid id)
    {
        var assessment = await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.AssessmentId == id).FirstOrDefaultAsync();
        if (assessment == null)
            return Result.Error("Assessment not found");
        return new(assessment);
    }

    public async Task<Result<Assessment>> UpdateAssessmentAsync(UpdateAssessmentRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetAllErrors());

        var assessment = Assessment.CreateInstance(request.AssessmentId, request.CourseId, request.Name, request.Type, request.StartDate, request.EndDate);
        var result = await ValidateAssessmentAsync(assessment);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().UpdateAsync(assessment);
        return assessment;
    }

    public async Task<Result> ValidateAssessmentAsync(Assessment assessment)
    {
        var existingCourse = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == assessment.CourseId).FirstOrDefaultAsync();
        if (existingCourse == null)
            return Result.Error("Course does not exist");

        var exisitingName = await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == assessment.CourseId && x.Name == assessment.Name).FirstOrDefaultAsync();
        if (exisitingName != null && exisitingName.AssessmentId != assessment.AssessmentId)
            return Result.Error("Assessment name already exists");

        var overlappingAssessments = await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == assessment.CourseId && x.StartDate <= assessment.EndDate && x.EndDate >= assessment.StartDate).ToListAsync();
        if (overlappingAssessments.Any(x => x.AssessmentId != assessment.AssessmentId))
            return Result.Error("Assessment overlaps with existing assessment");

        return Result.Success();
    }
}