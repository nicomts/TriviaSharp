using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TriviaSharp.OpenTDB;
using Xunit;
using System.Collections.Generic;

namespace TriviaSharp.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly string _responseContent;
    public MockHttpMessageHandler(string responseContent)
    {
        _responseContent = responseContent;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_responseContent)
        };
        return Task.FromResult(response);
    }
}

public class OpenTdbFetcherTests
{
    [Fact]
    public async Task GetTriviaQuestionsAsync_ReturnsExpectedQuestions()
    {
        string json = "{" +
            "\"response_code\":0," +
            "\"results\":[{" +
            "\"type\":\"multiple\"," +
            "\"difficulty\":\"hard\"," +
            "\"category\":\"Science: Computers\"," +
            "\"question\":\"Which of these Cherry MX mechanical keyboard switches is both tactile and clicky?\"," +
            "\"correct_answer\":\"Cherry MX Blue\"," +
            "\"incorrect_answers\":[\"Cherry MX Black\",\"Cherry MX Red\",\"Cherry MX Brown\"]},{" +
            "\"type\":\"multiple\"," +
            "\"difficulty\":\"hard\"," +
            "\"category\":\"Science: Computers\"," +
            "\"question\":\"According to DeMorgan&#039;s Theorem, the Boolean expression (AB)&#039; is equivalent to:\"," +
            "\"correct_answer\":\"A&#039; + B&#039;\"," +
            "\"incorrect_answers\":[\"A&#039;B + B&#039;A\",\"A&#039;B&#039;\",\"AB&#039; + AB\"]}]}";
        var mockHandler = new MockHttpMessageHandler(json);
        var mockClient = new HttpClient(mockHandler);
        OpenTdbFetcher.SetHttpClient(mockClient);
        var questions = await OpenTdbFetcher.GetTriviaQuestionsAsync(2, 18, "hard", "multiple");
        Assert.Equal(2, questions.Length);
        Assert.Equal("multiple", questions[0].Type);
        Assert.Equal("hard", questions[0].Difficulty);
        Assert.Equal("Science: Computers", questions[0].Category);
        Assert.Equal("Which of these Cherry MX mechanical keyboard switches is both tactile and clicky?", questions[0].QuestionText);
        Assert.Equal("Cherry MX Blue", questions[0].CorrectAnswer);
        Assert.Equal(new List<string> { "Cherry MX Black", "Cherry MX Red", "Cherry MX Brown" }, questions[0].IncorrectAnswers);
        Assert.Equal("multiple", questions[1].Type);
        Assert.Equal("hard", questions[1].Difficulty);
        Assert.Equal("Science: Computers", questions[1].Category);
        Assert.Equal("According to DeMorgan's Theorem, the Boolean expression (AB)' is equivalent to:", questions[1].QuestionText);
        Assert.Equal("A' + B'", questions[1].CorrectAnswer);
        Assert.Equal(new List<string> { "A'B + B'A", "A'B'", "AB' + AB" }, questions[1].IncorrectAnswers);
    }

    [Fact]
    public async Task GetSessionTokenAsync_ReturnsExpectedToken()
    {
        string json = "{\"response_code\":0,\"response_message\":\"Token Generated Successfully!\",\"token\":\"1f1cab7879167db1795e4d8af06f0980991e62074c970b45d8ed174d0cc7d395\"}";
        var mockHandler = new MockHttpMessageHandler(json);
        var mockClient = new HttpClient(mockHandler);
        OpenTdbFetcher.SetHttpClient(mockClient);
        var token = await OpenTdbFetcher.GetSessionTokenAsync();
        Assert.Equal("1f1cab7879167db1795e4d8af06f0980991e62074c970b45d8ed174d0cc7d395", token);
    }

    [Fact]
    public async Task GetTriviaCategoriesAsync_ReturnsExpectedCategories()
    {
        string json = "{\"trivia_categories\":[{\"id\":9,\"name\":\"General Knowledge\"},{\"id\":10,\"name\":\"Entertainment: Books\"},{\"id\":11,\"name\":\"Entertainment: Film\"},{\"id\":12,\"name\":\"Entertainment: Music\"},{\"id\":13,\"name\":\"Entertainment: Musicals & Theatres\"},{\"id\":14,\"name\":\"Entertainment: Television\"},{\"id\":15,\"name\":\"Entertainment: Video Games\"},{\"id\":16,\"name\":\"Entertainment: Board Games\"},{\"id\":17,\"name\":\"Science & Nature\"},{\"id\":18,\"name\":\"Science: Computers\"},{\"id\":19,\"name\":\"Science: Mathematics\"},{\"id\":20,\"name\":\"Mythology\"},{\"id\":21,\"name\":\"Sports\"},{\"id\":22,\"name\":\"Geography\"},{\"id\":23,\"name\":\"History\"},{\"id\":24,\"name\":\"Politics\"},{\"id\":25,\"name\":\"Art\"},{\"id\":26,\"name\":\"Celebrities\"},{\"id\":27,\"name\":\"Animals\"},{\"id\":28,\"name\":\"Vehicles\"},{\"id\":29,\"name\":\"Entertainment: Comics\"},{\"id\":30,\"name\":\"Science: Gadgets\"},{\"id\":31,\"name\":\"Entertainment: Japanese Anime & Manga\"},{\"id\":32,\"name\":\"Entertainment: Cartoon & Animations\"}]}";
        var mockHandler = new MockHttpMessageHandler(json);
        var mockClient = new HttpClient(mockHandler);
        OpenTdbFetcher.SetHttpClient(mockClient);
        var categories = await OpenTdbFetcher.GetTriviaCategoriesAsync();
        Assert.Equal(24, categories.Length);
        Assert.Equal(9, categories[0].Id);
        Assert.Equal("General Knowledge", categories[0].Name);
        Assert.Equal(18, categories[9].Id);
        Assert.Equal("Science: Computers", categories[9].Name);
    }

    public async Task RunFetcherTest()
    {
        try
        {
            await GetSessionTokenAsync_ReturnsExpectedToken();
            Console.WriteLine("OpenTdbFetcher GetSessionTokenAsync test passed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenTdbFetcher GetSessionTokenAsync test failed: {ex.Message}");
        }
        try
        {
            await GetTriviaCategoriesAsync_ReturnsExpectedCategories();
            Console.WriteLine("OpenTdbFetcher GetTriviaCategoriesAsync test passed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenTdbFetcher GetTriviaCategoriesAsync test failed: {ex.Message}");
        }
        try
        {
            await GetTriviaQuestionsAsync_ReturnsExpectedQuestions();
            Console.WriteLine("OpenTdbFetcher GetTriviaQuestionsAsync test passed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenTdbFetcher GetTriviaQuestionsAsync test failed: {ex.Message}");
        }
    }

    public async Task<bool> RunFetcherTestWithResult()
    {
        bool allPassed = true;
        try
        {
            await GetSessionTokenAsync_ReturnsExpectedToken();
            Console.WriteLine("OpenTdbFetcher GetSessionTokenAsync test passed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenTdbFetcher GetSessionTokenAsync test failed: {ex.Message}");
            allPassed = false;
        }
        try
        {
            await GetTriviaCategoriesAsync_ReturnsExpectedCategories();
            Console.WriteLine("OpenTdbFetcher GetTriviaCategoriesAsync test passed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenTdbFetcher GetTriviaCategoriesAsync test failed: {ex.Message}");
            allPassed = false;
        }
        try
        {
            await GetTriviaQuestionsAsync_ReturnsExpectedQuestions();
            Console.WriteLine("OpenTdbFetcher GetTriviaQuestionsAsync test passed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenTdbFetcher GetTriviaQuestionsAsync test failed: {ex.Message}");
            allPassed = false;
        }
        if (allPassed)
            Console.WriteLine("All OpenTdbFetcher tests passed successfully.");
        else
            Console.WriteLine("Some OpenTdbFetcher tests failed.");
        return allPassed;
    }
}