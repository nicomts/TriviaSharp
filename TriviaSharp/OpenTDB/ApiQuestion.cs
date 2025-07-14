namespace TriviaSharp.OpenTDB;
using System.Web;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the structure of a trivia question.
/// </summary>
public class ApiQuestion
{
    // Private backing fields for properties that need decoding
    private string _question;
    private string _correctAnswer;
    private List<string> _incorrectAnswers;

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("question")]
    public string QuestionText
    {
        get => _question;
        set => _question = HttpUtility.HtmlDecode(value); // Decode on set
    }

    [JsonPropertyName("correct_answer")]
    public string CorrectAnswer
    {
        get => _correctAnswer;
        set => _correctAnswer = HttpUtility.HtmlDecode(value); // Decode on set
    }

    [JsonPropertyName("incorrect_answers")]
    public List<string> IncorrectAnswers
    {
        get => _incorrectAnswers;
        set
        {
            if (value != null)
            {
                // Create a new list to store decoded answers
                _incorrectAnswers = new List<string>();
                foreach (var answer in value)
                {
                    _incorrectAnswers.Add(HttpUtility.HtmlDecode(answer)); // Decode each item
                }
            }
            else
            {
                _incorrectAnswers = null;
            }
        }
    }
}