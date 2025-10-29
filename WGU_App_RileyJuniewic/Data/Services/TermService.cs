using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ITermService
{
    Task<List<Term>> GetAllTermsAsync();
    Task<Term> GetTermAsync(Guid termId);
    Task<Term> CreateTermAsync(CreateTermRequest request);
    Task<Term> UpdateTermAsync(Term term);
    Task DeleteTermAsync(Guid termId);
}

public class TermService(SqlDataAccessAsync sqlDataAccess) : ITermService
{
    public async Task<Term> CreateTermAsync(CreateTermRequest request)
    {
        var term = Term.CreateNewInstance(request.Title, request.StartDate, request.EndDate);

        var existingTerms = await sqlDataAccess.GetConnection().Table<Term>().ToListAsync();
        foreach (var existingTerm in existingTerms)
        {
            if (existingTerm.StartDate <= term.EndDate && existingTerm.EndDate >= term.StartDate)
                throw new UserException("Term overlaps with an existing term");

            if (existingTerm.Title == term.Title) 
                throw new UserException("Term with the same title already exists");
        }

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

    public async Task<Term> GetTermAsync(Guid termId)
    {
        var term = await sqlDataAccess.GetConnection().Table<Term>().Where(x => x.TermId == termId).FirstOrDefaultAsync();
        if (term == null)
            throw new UserException("Term not found");
        return term;
    }

    public Task<List<Term>> GetAllTermsAsync() => sqlDataAccess.GetConnection().Table<Term>().ToListAsync();

    public async Task<Term> UpdateTermAsync(Term term)
    {
        await sqlDataAccess.GetConnection().UpdateAsync(term);
        return term;        
    }
}