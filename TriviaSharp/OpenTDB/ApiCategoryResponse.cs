namespace TriviaSharp.OpenTDB;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the full API response for categories.
/// </summary>
public class ApiCategoryResponse
{
    [JsonPropertyName("trivia_categories")]
    public ApiCategory[] TriviaCategories { get; set; }
}