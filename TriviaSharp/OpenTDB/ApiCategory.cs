namespace TriviaSharp.OpenTDB;
using System.Text.Json.Serialization;
using System.Web;

/// <summary>
/// Represents a single trivia category.
/// </summary>
public class ApiCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    private string _name;

    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set => _name = HttpUtility.HtmlDecode(value); // Decode HTML entities
    }
    
}