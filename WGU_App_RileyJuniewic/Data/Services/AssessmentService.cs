using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IAssessmentService
{
    Task<IEnumerable<Assessment>> GetAllAssessmentsAsync();
    Task<Assessment> GetAssessmentAsync(Guid id);
    Task<Assessment> CreateAssessmentAsync(CreateAssessmentRequest request);
    Task<Assessment> UpdateAssessmentAsync(UpdateAssessmentRequest request);
    Task DeleteAssessmentAsync(Guid id);
}

public class AssessmentService(SqlDataAccessAsync sqlDataAccess) : IAssessmentService
{
    public async Task<Assessment> CreateAssessmentAsync(CreateAssessmentRequest request)
    {
        var assessment = Assessment.CreateNewInstance(request.CourseId, request.Name, request.Type, request.StartDate, request.EndDate);
        await ValidateAssessmentAsync(assessment);
        await sqlDataAccess.GetConnection().InsertAsync(assessment);
        return assessment;
    }

    public async Task DeleteAssessmentAsync(Guid id) =>
        await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.AssessmentId == id).DeleteAsync();

    public async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync() => await sqlDataAccess.GetConnection().Table<Assessment>().ToListAsync();

    public async Task<Assessment> GetAssessmentAsync(Guid id)
    {
        var assessment = await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.AssessmentId == id).FirstOrDefaultAsync();
        if (assessment == null)
            throw new UserException("Assessment not found");
        return assessment;
    }

    public async Task<Assessment> UpdateAssessmentAsync(UpdateAssessmentRequest request)
    {
        var assessment = Assessment.CreateInstance(request.AssessmentId, request.CourseId, request.Name, request.Type, request.StartDate, request.EndDate);
        await ValidateAssessmentAsync(assessment);
        await sqlDataAccess.GetConnection().UpdateAsync(assessment);
        return assessment;
    }

    public async Task<bool> ValidateAssessmentAsync(Assessment assessment)
    {
        var existingCourse = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == assessment.CourseId).FirstOrDefaultAsync();
        if (existingCourse == null)
            throw new UserException("Course does not exist");

        var exisitingName = await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == assessment.CourseId && x.Name == assessment.Name).FirstOrDefaultAsync();
        if (exisitingName != null && exisitingName.AssessmentId != assessment.AssessmentId)
            throw new UserException("Assessment name already exists");

        var overlappingAssessments = await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == assessment.CourseId && x.StartDate <= assessment.EndDate && x.EndDate >= assessment.StartDate).ToListAsync();
        if (overlappingAssessments.Any(x => x.AssessmentId != assessment.AssessmentId))
            throw new UserException("Assessment overlaps with existing assessment");

        return true;
    }
}