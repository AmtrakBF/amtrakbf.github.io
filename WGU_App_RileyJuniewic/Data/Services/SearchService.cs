using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ISearchService
{
    Task<SearchResult> SearchAllAsync(string searchQuery, Guid userId);
}

public class SearchService(SqlDataAccessAsync sqlDataAccess) : ISearchService
{
    public async Task<SearchResult> SearchAllAsync(string searchQuery, Guid userId)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var like = $"%{searchQuery.ToLower()}%";

        if (searchQuery.Trim() == "")
            return new SearchResult();

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
            INNER JOIN Assessment a ON c.CourseId = a.CourseId
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
            INNER JOIN Assessment a ON c.CourseId = a.CourseId
            INNER JOIN Term t ON c.TermId = t.TermId
            WHERE (
                LOWER(i.Name) LIKE ? OR
                LOWER(i.Email) LIKE ? OR
                LOWER(i.Phone) LIKE ?
            )
            AND t.UserId = ? 
        ";


        var assessments = await connection.QueryAsync<Assessment>(queryAssessment, like, like, userId);
        var terms = await connection.QueryAsync<Term>(queryTerms, like, userId);
        var courses = await connection.QueryAsync<Course>(queryCourses, like, like, like, userId);
        var instructors = await connection.QueryAsync<Instructor>(queryInstructors, like, like, like, userId);

        return new SearchResult()
        {
            Assessments = assessments,
            Terms = terms,
            Courses = courses,
            Instructors = instructors
        };
    }
}