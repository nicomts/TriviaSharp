namespace TriviaSharp.Tests;

public static class UnitTestsRunner
{
    public static void RunAllTests()
    {
        // User service tests
        var userServiceTests = new UserServiceTests();
        userServiceTests.RegisterAsync_ShouldReturnTrue_WhenUserIsRegisteredSuccessfully().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.RegisterAsync_ShouldReturnFalse_WhenUserAlreadyExists().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.LoginAsync_ShouldReturnUser_WhenCredentialsAreValid().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.LoginAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.LoginAsync_ShouldReturnIncorrectPassword_WhenPasswordIsInvalid().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.ChangePasswordAsync_ShouldReturnTrue_WhenPasswordIsChangedSuccessfully().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.ChangePasswordAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist().GetAwaiter().GetResult();
        userServiceTests = new UserServiceTests();
        userServiceTests.ChangePasswordAsync_ShouldReturnIncorrectPassword_WhenOldPasswordIsInvalid().GetAwaiter().GetResult();
    }
}