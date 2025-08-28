namespace TriviaSharp.Models;

public class QuizSession
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public User User { get; set; }
    public int Score { get; set; }
    public double TimeTakenSeconds { get; set; }
    public string Difficulty { get; set; }
    public ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}
