namespace TriviaSharp.Models;
using Models.Enums;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Regular;
    public ICollection<QuizSession> Sessions { get; set; } = new List<QuizSession>();
}
