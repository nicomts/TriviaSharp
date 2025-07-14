namespace TriviaSharp.OpenTDB;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the API response structure.
/// </summary>
public class ApiResponse
{
    [JsonPropertyName("response_code")]
    public int ResponseCode { get; set; }

    [JsonPropertyName("results")]
    public List<ApiQuestion> ApiQuestions { get; set; }
}