using TriviaSharp.Utils;

namespace TriviaSharp.OpenTDB;

using System.Web;
using System.Text.Json;

public class OpenTdbFetcher
{
    private static HttpClient _httpClient = new HttpClient();
    private const string BaseApiUrl = "https://opentdb.com/api.php";
    private const string TokenApiUrl = "https://opentdb.com/api_token.php";
    private const string CategoriesApiUrl = "https://opentdb.com/api_category.php";

    /// <summary>
    /// Fetches a session token from the OpenTDB API.
    /// </summary>
    /// <returns>The session token string.</returns>
    /// <exception cref="Exception">Thrown if there's an error fetching the token.</exception>
    public static async Task<string> GetSessionTokenAsync()
    {
        GlobalConfig.Logger.Information("OpenTdbFetcher: Attempting to fetch session token from OpenTDB API");
        try
        {
            var response = await _httpClient.GetStringAsync($"{TokenApiUrl}?command=request");
            using (JsonDocument doc = JsonDocument.Parse(response))
            {
                if (doc.RootElement.TryGetProperty("response_code", out JsonElement codeElement) &&
                    doc.RootElement.TryGetProperty("token", out JsonElement tokenElement))
                {
                    int responseCode = codeElement.GetInt32();
                    string token = tokenElement.GetString();

                    if (responseCode == 0)
                    {
                        GlobalConfig.Logger.Information($"OpenTdbFetcher: Session token retrieved. Token: {token}");
                        return token;
                    }
                    else
                    {
                        GlobalConfig.Logger.Error($"Error getting session token. Response Code: {responseCode}");
                        throw new Exception($"Error getting session token. Response Code: {responseCode}");
                    }
                }
                else
                {
                    GlobalConfig.Logger.Error("Invalid response format when fetching session token.");
                    throw new Exception("Invalid response format when fetching session token.");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            GlobalConfig.Logger.Error($"Network error fetching session token: {ex.Message}");
            throw new Exception($"Network error fetching session token: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            GlobalConfig.Logger.Error($"JSON parsing error fetching session token: {ex.Message}");
            throw new Exception($"JSON parsing error fetching session token: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Fetches the list of trivia categories from the OpenTDB API.
    /// </summary>
    /// <returns>An array of ApiCategory objects.</returns>
    /// <exception cref="Exception">Thrown if there's an error fetching or deserializing the data.</exception>
    public static async Task<ApiCategory[]> GetTriviaCategoriesAsync()
    {
        GlobalConfig.Logger.Information("OpenTdbFetcher: Attempting to fetch trivia categories from OpenTDB API");
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(CategoriesApiUrl);
            response.EnsureSuccessStatusCode(); // Throws an exception for 4xx or 5xx status codes

            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string into the ApiCategoryResponse object
            var apiResponse = JsonSerializer.Deserialize<ApiCategoryResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false // Set to false because "TriviaCategories" exactly matches "trivia_categories" in JSON
            });

            if (apiResponse?.TriviaCategories == null)
            {
                throw new Exception("Failed to deserialize categories or no categories found in the response.");
            }

            GlobalConfig.Logger.Information("OpenTdbFetcher: Trivia categories retrieved");
            return apiResponse.TriviaCategories;
        }
        catch (HttpRequestException ex)
        {
            GlobalConfig.Logger.Error($"Error fetching categories: {ex.Message}");
            throw new Exception($"Network error fetching categories: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            GlobalConfig.Logger.Error($"Error fetching categories: {ex.Message}");
            throw new Exception($"Error deserializing JSON categories: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Re-throw any other exceptions
            GlobalConfig.Logger.Error($"Error fetching categories: {ex.Message}");
            throw new Exception($"An unexpected error occurred while fetching categories: {ex.Message}", ex);
        }
    }
    
    
    /// <summary>
    /// Retrieves trivia questions from the OpenTDB API.
    /// </summary>
    /// <param name="amount">Number of questions (max 50).</param>
    /// <param name="category">Category ID (e.g., 9 for General Knowledge).</param>
    /// <param name="difficulty">Difficulty: "easy", "medium", "hard".</param>
    /// <param name="type">Type: "multiple" or "boolean".</param>
    /// <param name="sessionToken">Optional session token to prevent duplicate questions.</param>
    /// <param name="encoding">Encoding: "default", "urlLegacy", "url3986", "base64".</param>
    /// <returns>An array of ApiQuestion objects.</returns>
    /// <exception cref="Exception">Thrown in case of API errors or invalid parameters.</exception>
    public static async Task<ApiQuestion[]> GetTriviaQuestionsAsync(
        int amount,
        int? category = null,
        string difficulty = null,
        string type = null,
        string sessionToken = null,
        string encoding = null)
    {
        if (amount < 1 || amount > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be between 1 and 50.");
        }

        GlobalConfig.Logger.Information($"OpenTdbFetcher: Fetching {amount} trivia questions with category: {category}, difficulty: {difficulty}, type: {type}, sessionToken: {sessionToken}, encoding: {encoding}");
        var uriBuilder = new UriBuilder(BaseApiUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["amount"] = amount.ToString();

        if (category.HasValue)
        {
            query["category"] = category.Value.ToString();
        }
        if (!string.IsNullOrEmpty(difficulty))
        {
            query["difficulty"] = difficulty;
        }
        if (!string.IsNullOrEmpty(type))
        {
            query["type"] = type;
        }
        if (!string.IsNullOrEmpty(sessionToken))
        {
            query["token"] = sessionToken;
        }
        if (!string.IsNullOrEmpty(encoding))
        {
            query["encode"] = encoding;
        }

        uriBuilder.Query = query.ToString();
        string apiUrl = uriBuilder.ToString();
        GlobalConfig.Logger.Information($"OpenTdbFetcher: Constructed API URL: {apiUrl}");


        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP status code is an error

            string jsonResponse = await response.Content.ReadAsStringAsync();
            
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allows matching JSON "response_code" to C# "ResponseCode"
            });

            if (apiResponse == null)
            {
                throw new Exception("Failed to deserialize API response.");
            }
            GlobalConfig.Logger.Information($"OpenTdbFetcher: API response received with code: {apiResponse.ResponseCode}");

            switch (apiResponse.ResponseCode)
            {
                case 0: // Success
                    GlobalConfig.Logger.Information("OpenTdbFetcher: Questions retrieved successfully");
                    return apiResponse.ApiQuestions.ToArray();
                case 1: // No Results
                    throw new Exception("No questions could be returned for the specified query (Code 1).");
                case 2: // Invalid Parameter
                    throw new Exception("Contains an invalid parameter (Code 2).");
                case 3: // Token Not Found
                    throw new Exception("Session Token does not exist (Code 3).");
                case 4: // Token Empty
                    throw new Exception("Session Token has returned all possible questions (Code 4). Reset or request a new token.");
                case 5: // Rate Limit
                     throw new Exception("Rate limit reached. Only one request per IP every 5 seconds (Code 5).");
                default:
                    throw new Exception($"Unknown API response code: {apiResponse.ResponseCode}.");
            }
        }
        catch (HttpRequestException ex)
        {
            GlobalConfig.Logger.Error($"Error fetching questions: {ex.Message}");
            throw new Exception($"Network error or non-success HTTP status: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            GlobalConfig.Logger.Error($"Error fetching questions: {ex.Message}");
            throw new Exception($"Error deserializing JSON response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            GlobalConfig.Logger.Error($"Error fetching questions: {ex.Message}");
            throw ex; // Re-throw specific API exceptions
        }
    }

    /// <summary>
    /// Disposes the HttpClient instance to save resources.
    /// </summary>
    public static void HttpClientDispose()
    {
        _httpClient.Dispose();
    }

    /// <summary>
    /// Sets a custom HttpClient instance. This is mainly for testing purposes.
    /// </summary>
    /// <param name="client">The HttpClient instance to be used.</param>
    public static void SetHttpClient(HttpClient client)
    {
        _httpClient = client;
    }
}