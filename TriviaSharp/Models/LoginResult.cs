using TriviaSharp.Models.Enums;

namespace TriviaSharp.Models;

public class LoginResult
{
    public LoginStatus Status { get; set; }
    public User? User { get; set; }
}