using Ardalis.Result;
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class UserServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] protected IUserService _userService { get; set; } = null!;
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;
    
    public UserServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateUserAsync_CreatesDBEntryAsync()
    {
        await ClearUsers();

        var request = new UserCreateRequest()
        {
            Username = "Test User",
            Password = "mypassword",
            ConfirmPassword = "mypassword"
        };

        var result = await _userService.CreateAsync(request);
        result.IsError().Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value!.Username.Should().Be(request.Username.ToLower());
        result.Value!.Password.Should().NotBe(request.Password);
        result.Value.MatchPassword(request.Password).Should().BeTrue();
    }

    [Theory]
    [InlineData("Test User", "mypassword", "mypassword")]
    [InlineData("Test User", "mypassword", "mypassword", "User already exists.")]
    [InlineData("Test User2", "mypassword", "notvalid", "Passwords do not match.")]
    public async Task CreateUserAsync_WithInvalidData_ReturnsErrorAsync(string username, string password, string confirmPassword, string? errorMessage = null)
    {
        if (errorMessage == null)
            await ClearUsers();

        var request = new UserCreateRequest()
        {
            Username = username,
            Password = password,
            ConfirmPassword = confirmPassword
        };

        if (errorMessage != null)
        {
            var result = await _userService.CreateAsync(request);
            result.IsError().Should().BeTrue();
            result.Errors.First().Should().Be(errorMessage);   
        } else
        {
            var result = await _userService.CreateAsync(request);
            result.IsError().Should().BeFalse();
            result.Value.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task GetUserAsync_ReturnsUserAsync()
    {
        await ClearUsers();

        var request = new UserCreateRequest()
        {
            Username = "Test User",
            Password = "mypassword",
            ConfirmPassword = "mypassword"
        };

        var result = await _userService.CreateAsync(request);
        result.IsError().Should().BeFalse();

        var userResult = await _userService.GetAsync(result.Value.UserId);
        userResult.IsError().Should().BeFalse();
        userResult.Value.Username.Should().Be(request.Username.ToLower());
        userResult.Value.Password.Should().NotBe(request.Password);
        userResult.Value.MatchPassword(request.Password).Should().BeTrue();
    }

    [Fact]
    public async Task GetUserAsync_WithInvalidId_ReturnsErrorAsync()
    {
        await ClearUsers();

        var result = await _userService.GetAsync(Guid.NewGuid());
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("User not found.");
    }

    [Fact]
    public async Task LoginAsync_ReturnsUserAsync()
    {
        await ClearUsers();

        var userRequest = new UserCreateRequest()
        {
            Username = "Test User",
            Password = "mypassword",
            ConfirmPassword = "mypassword"
        };

        var userResult = await _userService.CreateAsync(userRequest);
        userResult.IsError().Should().BeFalse();

        var request = new UserLoginRequest()
        {
            Username = "Test User",
            Password = "mypassword",
        };

        var loginUserResult = await _userService.LoginAsync(request);
        loginUserResult.IsError().Should().BeFalse();
        loginUserResult.Value.Username.Should().Be(request.Username.ToLower());
        loginUserResult.Value.Password.Should().NotBe(request.Password);
        loginUserResult.Value.MatchPassword(request.Password).Should().BeTrue();
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsErrorAsync()
    {
        await ClearUsers();

        var request = new UserLoginRequest()
        {
            Username = "Test User",
            Password = "mypassword",
        };

        var userResult = await _userService.LoginAsync(request);
        userResult.IsError().Should().BeTrue();
        userResult.Errors.First().Should().Be("Invalid username or password.");
    }

    private async Task ClearUsers()
    {
        var connection = await _dbAccessAsync.GetConnectionAsync();
        await connection.DeleteAllAsync<User>();
    }
}
