namespace TriviaSharp.Providers;
using TriviaSharp.OpenTDB;

public interface ITriviaProvider
{
    Task<ApiCategory[]> GetCategories();
    Task GetQuestions(int amount,
        int? category = null,
        string difficulty = null,
        string type = null,
        string encoding = null);
    Task SetupAsync();
}