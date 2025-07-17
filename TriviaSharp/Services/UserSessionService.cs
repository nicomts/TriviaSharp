namespace TriviaSharp.Services;
using TriviaSharp.Models;

public class UserSessionService
{
    private static UserSessionService? _instance;
    public static UserSessionService Instance => _instance ??= new UserSessionService();

    public User? CurrentUser { get; set; }

    private UserSessionService() { }
}