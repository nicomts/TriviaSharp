using TriviaSharp.Models;
using TriviaSharp.Utils;

namespace TriviaSharp.Services;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models.Enums;
using BCrypt.Net;

public class UserService
{
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Constructor for UserService.
    /// </summary>
    /// <param name="userRepository">An instance of IUserRepository to interact with user data.</param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Registers a new user with the specified username, password, and role.
    /// </summary>
    /// <param name="username">The username of the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="role">The role of the new user (e.g., Regular, Admin).</param>
    /// <returns>A boolean indicating whether the registration was successful. If user already exists, returns false</returns>
    public async Task<bool> RegisterAsync(string username, string password, UserRole role)
    {
        GlobalConfig.Logger.Information($"UserService: Attempting to register user with username: {username} and role: {role}");
        
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser != null)
        {
            GlobalConfig.Logger.Error($"UserService: Registration failed for username: {username}");
            return false; // User already exists
        }
        
        try
        {
            var hashedPassword = BCrypt.HashPassword(password);
            var newUser = new Models.User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = role
            };
            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
            GlobalConfig.Logger.Information($"UserService: Registration successful for username: {username}");
            return true; // Registration successful
        }
        catch (Exception ex)
        {
            GlobalConfig.Logger.Error(ex, $"UserService: Registration failed for username: {username}");
            throw;
        } 
        

    }

    /// <summary>
    /// Logs in a user with the specified username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A LoginResult indicating the user and the status of the login attempt. e.g., Success, UserNotFound, IncorrectPassword.</returns>
    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        GlobalConfig.Logger.Information($"UserService: Attempting to log in user with username: {username}");
        
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            GlobalConfig.Logger.Warning($"UserService: Login failed - user not found for username: {username}");
            return new LoginResult { Status = LoginStatus.UserNotFound };
        }
        
        bool isPasswordCorrect = BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            GlobalConfig.Logger.Warning($"UserService: Login failed - incorrect password for username: {username}");
            return new LoginResult { Status = LoginStatus.IncorrectPassword };
        }
        
        GlobalConfig.Logger.Information($"UserService: Login successful for username: {username}");
        return new LoginResult
        {
            Status = LoginStatus.Success,
            User = user
        }; // Login successful
        
    }
    
    /// <summary>
    /// Changes the password for a user with the specified username.
    /// </summary>
    /// <param username="username">The username of the user whose password is to be changed.</param>
    /// <param oldPassword="oldPassword">The current password of the user.</param>
    /// <param newPassword="newPassword">The new password to set for the user.</param>
    public async Task<ChangePasswordStatus> ChangePasswordAsync(string username, string oldPassword, string newPassword)
    {
        GlobalConfig.Logger.Information($"UserService: Attempting to change password for username: {username}");
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            GlobalConfig.Logger.Warning($"UserService: Change password failed - user not found for username: {username}");
            return ChangePasswordStatus.UserNotFound; // User not found
        }
        
        bool isOldPasswordCorrect = BCrypt.Verify(oldPassword, user.PasswordHash);
        if (!isOldPasswordCorrect)
        {
            GlobalConfig.Logger.Warning($"UserService: Change password failed - incorrect old password for username: {username}");
            return ChangePasswordStatus.IncorrectPassword; // Incorrect old password
        }
        
        user.PasswordHash = BCrypt.HashPassword(newPassword);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        
        GlobalConfig.Logger.Information($"UserService: Password changed successfully for username: {username}");
        return ChangePasswordStatus.Success; // Password changed successfully
    }
}