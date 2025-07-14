namespace TriviaSharp.OpenTDB;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a single trivia category.
/// </summary>
public class ApiCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}