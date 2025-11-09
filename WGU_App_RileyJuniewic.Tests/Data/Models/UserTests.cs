
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Tests.Data.Models;

public class UserTests : BaseTest
{
    public UserTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateUser_CreatesDBEntryAsync()
    {
        var user = User.CreateNewInstance("Test User", "mypassword");
        
        var connection = await _dbAccessAsync.GetConnectionAsync();
        await connection.InsertAsync(user);
        var userFromDb = await connection.Table<User>().Where(x => x.UserId == user.UserId).FirstOrDefaultAsync();
        userFromDb.Should().BeEquivalentTo(user);
    }
}
