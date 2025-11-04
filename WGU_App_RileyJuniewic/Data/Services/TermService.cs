using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ITermService
{
    Task<List<Term>> GetAllTermsAsync();
    Task<Result<Term>> GetTermAsync(Guid termId);
    Task<Result<Term>> CreateTermAsync(CreateTermRequest request);
    Task<Result<Term>> UpdateTermAsync(UpdateTermRequest request);
    Task DeleteTermAsync(Guid termId);
}

public class TermService(SqlDataAccessAsync sqlDataAccess) :
    ITermService,
    ICreateService<CreateTermRequest, Term>,
    IModifyService<UpdateTermRequest, Term>
{
    public async Task<Result<Term>> CreateTermAsync(CreateTermRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var term = Term.CreateNewInstance(request.Title, request.StartDate, request.EndDate);
        var result = await ValidateTermAsync(term);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().InsertAsync(term);
        return term;
    }

    public async Task DeleteTermAsync(Guid termId)
    {
        // Delete Assesements & Notes
        var courses = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.TermId == termId).ToListAsync();
        foreach (var course in courses)
        {
            await sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.CourseId == course.CourseId).DeleteAsync();
            await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.CourseId == course.CourseId).DeleteAsync();
        }
        // Delete Courses
        await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.TermId == termId).DeleteAsync();

        // Delete Term
        await sqlDataAccess.GetConnection().Table<Term>().Where(x => x.TermId == termId).DeleteAsync();
    }

    public async Task<Result<Term>> GetTermAsync(Guid termId)
    {
        var term = await sqlDataAccess.GetConnection().Table<Term>().Where(x => x.TermId == termId).FirstOrDefaultAsync();
        if (term == null)
            return Result.Error("Term not found");

        return term;
    }

    public Task<List<Term>> GetAllTermsAsync() => sqlDataAccess.GetConnection().Table<Term>().ToListAsync();

    public async Task<Result<Term>> UpdateTermAsync(UpdateTermRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());
            
        var term = Term.CreateInstance(request.Id, request.Title, request.StartDate, request.EndDate);
        var result = await ValidateTermAsync(term);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().UpdateAsync(term);
        return term;
    }

    internal async Task<Result> ValidateTermAsync(Term term)
    {
        var existingTerms = await sqlDataAccess.GetConnection().Table<Term>().ToListAsync();
        foreach (var existingTerm in existingTerms)
        {
            if (existingTerm.StartDate <= term.EndDate && existingTerm.EndDate >= term.StartDate && existingTerm.TermId != term.TermId)
                return Result.Error("Term overlaps with an existing term");

            if (existingTerm.Title == term.Title && existingTerm.TermId != term.TermId)
                return Result.Error("Term with the same title already exists");
        }

        var monthSpan = term.EndDate.Month - term.StartDate.Month;
        if (monthSpan > 6)
            return Result.Error("Term must be less than 6 months");

            return Result.Success();
    }

    public async Task<Result<Term>> CreateAsync(CreateTermRequest request) => await CreateTermAsync(request);

    public async Task<Result<Term>> UpdateAsync(UpdateTermRequest request) => await UpdateTermAsync(request);

    public async Task DeleteAsync(Guid id) => await DeleteTermAsync(id);
}