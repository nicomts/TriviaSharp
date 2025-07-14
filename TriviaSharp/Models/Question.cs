namespace TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Category Category { get; set; }
    public Difficulty Difficulty { get; set; }
    public int QuestionSetId { get; set; }
    public QuestionSet QuestionSet { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
