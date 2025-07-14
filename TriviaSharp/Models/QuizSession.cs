namespace TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public class QuizSession
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    public User User { get; set; }
    public int Score { get; set; }
    public double TimeTakenSeconds { get; set; }
    public Difficulty Difficulty { get; set; }
}
