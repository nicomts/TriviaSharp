namespace TriviaSharp.Models;

public class QuizAnswer
{
    public int Id { get; set; }
    public int QuizSessionId { get; set; }
    public int QuestionId { get; set; }
    public int SelectedOptionId { get; set; }
}