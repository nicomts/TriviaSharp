namespace TriviaSharp.Providers;

using TriviaSharp.OpenTDB;
using TriviaSharp.Utils;

public class OpenTdbTriviaProvider : ITriviaProvider
{
    public async Task<ApiCategory[]> GetCategories()
    {
        var categories = await TriviaSharp.OpenTDB.OpenTdbFetcher.GetTriviaCategoriesAsync();
        return categories;
    }

    public async Task GetQuestions(int amount,
        int? category = null,
        string difficulty = null,
        string type = null,
        string encoding = null)
    {
        var questions = await OpenTdbFetcher.GetTriviaQuestionsAsync(
            amount, category, difficulty, type, GlobalConfig.SessionToken, encoding);
        await GlobalConfig.OpenTdbService.ImportApiQuestions(questions);
    }

    public async Task SetupAsync()
    {
        GlobalConfig.SessionToken = await TriviaSharp.OpenTDB.OpenTdbFetcher.GetSessionTokenAsync();
    }
}
