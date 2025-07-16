using TriviaSharp.Models;

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
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser != null)
        {
            return false; // User already exists
        }
        var hashedPassword = BCrypt.HashPassword(password);
        var newUser = new Models.User
        {
            Username = username,
            PasswordHash = hashedPassword,
            Role = role
        };
        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();
        return true; // Registration successful
    }

    /// <summary>
    /// Logs in a user with the specified username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A LoginResult indicating the user and the status of the login attempt. e.g., Success, UserNotFound, IncorrectPassword.</returns>
    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return new LoginResult { Status = LoginStatus.UserNotFound };
        }
        
        bool isPasswordCorrect = BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            return new LoginResult { Status = LoginStatus.IncorrectPassword };
        }
        
        return new LoginResult
        {
            Status = LoginStatus.Success,
            User = user
        }; // Login successful
        
    }
}