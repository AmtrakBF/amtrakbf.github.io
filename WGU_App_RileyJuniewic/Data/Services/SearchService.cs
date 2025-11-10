using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ISearchService
{
    Task<Result<SearchResult>> SearchAllAsync(string searchQuery);
}

public class SearchService(SqlDataAccessAsync sqlDataAccess, UserStore userStore) : ISearchService
{
    public async Task<Result<SearchResult>> SearchAllAsync(string searchQuery)
    {
        var userResult = userStore.GetUser();
        if (userResult.IsError())
            return Result.Error(new ErrorList(userResult.Errors));

        var connection = await sqlDataAccess.GetConnectionAsync();
        var like = $"%{searchQuery.ToLower()}%";

        if (searchQuery.Trim() == "")
            return Result.Error("Search query cannot be empty");

        var queryAssessment = @"
            SELECT DISTINCT a.* FROM Assessment a
            INNER JOIN Course c ON a.CourseId = c.CourseId
            INNER JOIN Term t ON c.TermId = t.TermId
            WHERE (
                LOWER(a.Name) LIKE ? OR
                LOWER(a.Type) LIKE ?
            )
            AND t.UserId = ?
        ";

        var queryTerms = @"
            SELECT DISTINCT t.* FROM Term t
            WHERE (
                LOWER(t.Title) LIKE ?
            )
            AND t.UserId = ?
        ";

        var queryCourses = @"
            SELECT DISTINCT c.* FROM Course c
            INNER JOIN Term t ON c.TermId = t.TermId
            WHERE (
                LOWER(c.Title) LIKE ? OR
                LOWER(c.Notes) LIKE ? OR
                LOWER(c.Status) LIKE ?
            )
            AND t.UserId = ? 
        ";

        var queryInstructors = @"
            SELECT DISTINCT i.* FROM Instructor i
            INNER JOIN Course c ON i.InstructorId = c.InstructorId
            INNER JOIN Term t ON c.TermId = t.TermId
            WHERE (
                LOWER(i.Name) LIKE ? OR
                LOWER(i.Email) LIKE ? OR
                LOWER(i.Phone) LIKE ?
            )
            AND t.UserId = ? 
        ";


        var assessments = await connection.QueryAsync<Assessment>(queryAssessment, like, like, userResult.Value.UserId);
        var terms = await connection.QueryAsync<Term>(queryTerms, like, userResult.Value.UserId);
        var courses = await connection.QueryAsync<Course>(queryCourses, like, like, like, userResult.Value.UserId);
        var instructors = await connection.QueryAsync<Instructor>(queryInstructors, like, like, like, userResult.Value.UserId);

        return new SearchResult()
        {
            Assessments = assessments,
            Terms = terms,
            Courses = courses,
            Instructors = instructors
        };
    }
}