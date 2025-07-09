namespace TriviaSharp.Models;

public class QuestionSet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
