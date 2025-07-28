using TriviaSharp.Utils;

namespace TriviaSharp.Tests;
using TriviaSharp.Services;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models;
using Moq;
using Xunit;


public class QuizServiceTests
{
    private readonly QuizService _quizService;
    public QuizServiceTests()
    {
        _quizService = new QuizService();
    }
    
    // Write a test for the equation in calulateScoreAsync method
    [Fact]
    public void CalculateScore_ShouldReturnCorrectScore_WhenGivenValidParameters()
    {
        // Arrange
        var correctAnswers = 10;
        var totalQuestions = 10;
        var difficulty = "easy";
        var time = 30.0; // seconds
        // 10/10 * 1 * 5 * 100
        var expectedScore = 500;

        // Act
        var score = _quizService.CalculateScore(totalQuestions, correctAnswers, difficulty, time);

        // Assert
        Assert.Equal(expectedScore, score);
        
        // Print test result to console
        Console.WriteLine($"Test CalculateScoreAsync_ShouldReturnCorrectScore_WhenGivenValidParameters: {score}");
    }
    
    
}