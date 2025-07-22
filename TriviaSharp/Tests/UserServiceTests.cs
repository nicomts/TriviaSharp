using Xunit;
using Moq;
using TriviaSharp.Services;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;


namespace TriviaSharp.Tests;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnTrue_WhenUserIsRegisteredSuccessfully()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var role = UserRole.Regular;

        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User)null);

        // Act
        var result = await _userService.RegisterAsync(username, password, role);

        // Assert
        Assert.True(result);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        
        // Print test result to console
        Console.WriteLine($"Test RegisterAsync_ShouldReturnTrue_WhenUserIsRegisteredSuccessfully: {result}");
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFalse_WhenUserAlreadyExists()
    {
        // Arrange
        var username = "existinguser";
        var password = "password123";
        var role = UserRole.Regular;

        var existingUser = new User { Username = username };
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(existingUser);

        // Act
        var result = await _userService.RegisterAsync(username, password, role);

        // Assert
        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        
        // Print test result to console
        Console.WriteLine($"Test RegisterAsync_ShouldReturnFalse_WhenUserAlreadyExists: {result}");
    }
    
    // User login tests
    // Add more tests for user login, password hashing, etc. as needed
    // Example of a login test
    [Fact]
    public async Task LoginAsync_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var username = "validuser";
        var password = "validpassword";
        var role = UserRole.Regular;

        var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), Role = role };
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.User.Username);
        
        // Print test result to console
        Console.WriteLine($"Test LoginAsync_ShouldReturnUser_WhenCredentialsAreValid: {result != null} User: {result.User.Username}");
    }
    
    // Add user not found test
    [Fact]
    public async Task LoginAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "nonexistentuser";
        var password = "password123";

        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User)null);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(LoginStatus.UserNotFound, result.Status);
        
        // Print test result to console
        Console.WriteLine($"Test LoginAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist: {result.Status}");
    }
    
    // Add incorrect password test
    [Fact]
    public async Task LoginAsync_ShouldReturnIncorrectPassword_WhenPasswordIsInvalid()
    {
        // Arrange
        var username = "validuser";
        var password = "wrongpassword";
        var role = UserRole.Regular;

        var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"), Role = role };
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(LoginStatus.IncorrectPassword, result.Status);
        
        // Print test result to console
        Console.WriteLine($"Test LoginAsync_ShouldReturnIncorrectPassword_WhenPasswordIsInvalid: {result.Status}");
    }
    
    // Add change password test
    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnTrue_WhenPasswordIsChangedSuccessfully()
    {
        // Arrange
        var username = "testuser";
        var oldPassword = "oldpassword";
        var newPassword = "newpassword";

        var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(oldPassword) };
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);

        // Act
        var result = await _userService.ChangePasswordAsync(username, oldPassword, newPassword);

        // Assert
        Assert.Equal(ChangePasswordStatus.Success, result);
        _userRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        
        // Print test result to console
        Console.WriteLine($"Test ChangePasswordAsync_ShouldReturnTrue_WhenPasswordIsChangedSuccessfully: {result}");
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "nonexistentuser";
        var oldPassword = "oldpassword";
        var newPassword = "newpassword";
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User)null);
        // Act
        var result = await _userService.ChangePasswordAsync(username, oldPassword, newPassword);
        // Assert
        Assert.Equal(ChangePasswordStatus.UserNotFound, result);
        _userRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        // Print test result to console
        Console.WriteLine($"Test ChangePasswordAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist: {result}");
    }
    
    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnIncorrectPassword_WhenOldPasswordIsInvalid()
    {
        // Arrange
        var username = "testuser";
        var oldPassword = "wrongoldpassword";
        var newPassword = "newpassword";

        var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctoldpassword") };
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);

        // Act
        var result = await _userService.ChangePasswordAsync(username, oldPassword, newPassword);

        // Assert
        Assert.Equal(ChangePasswordStatus.IncorrectPassword, result);
        _userRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        
        // Print test result to console
        Console.WriteLine($"Test ChangePasswordAsync_ShouldReturnIncorrectPassword_WhenOldPasswordIsInvalid: {result}");
    }
}