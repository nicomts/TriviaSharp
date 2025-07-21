using TriviaSharp.Utils;

namespace TriviaSharp.Services;
using TriviaSharp.Models;

public class QuizService
{
    // This service controls the quiz logic, including starting a quiz, and calculating scores.
    
    /// <summary>
    /// Gets a random set of questions for a quiz based on the specified category, difficulty, and number of questions.
    /// </summary>
    /// <param name="category">Selected category.</param>
    /// <param name="difficulty">Selected difficulty.</param>
    /// <param name="numberOfQuestions">Total number of questions.</param>
    /// <param name="questionSet">Question Set.</param>
    /// <returns>A list of random questions.</returns>
    public async Task<List<Question>> GetQuizAsync(Category category, string difficulty, int numberOfQuestions, QuestionSet questionSet)
    {
        GlobalConfig.Logger.Information($"QuizService: Attempting to get {numberOfQuestions} random questions for category: {category.Name}, difficulty: {difficulty}, question set: {questionSet.Name}");
        // Get random questions from the repository based on category, difficulty, amount and question set
        var randomQuestions = (await GlobalConfig.QuestionRepo.GetByCategoryAndDifficultyAsync(category, difficulty, questionSet))
            .OrderBy(q => Guid.NewGuid()).Take(numberOfQuestions).ToList();
        

        
        // For each question in randomQuestions, if one of the answers is "True" or "False", order the answers so that "True" is always first and "False" is always second. Else, random sort answers
        foreach (var question in randomQuestions)
        {
            // Get the value of the first answer from the list of answers of question
            var firstAnswer = question.Answers.FirstOrDefault()?.Text;
            if (firstAnswer == "True" || firstAnswer == "False")
            {
                question.Answers = question.Answers.OrderBy(a => a.Text == "True" ? 0 : a.Text == "False" ? 1 : 2).ToList();
            }
            else
            {
                question.Answers = question.Answers.OrderBy(a => Guid.NewGuid()).ToList();
            }
        }
        
        
        
        // Return the random questions
        GlobalConfig.Logger.Information($"QuizService: Retrieved {randomQuestions.Count} questions");
        return randomQuestions;
        
    }

    /// <summary>
    /// Calculates the score based on the total number of questions, correct answers, difficulty level, and time taken.
    /// </summary>
    /// <param name="totalQuestions"></param>
    /// <param name="correctAnswers"></param>
    /// <param name="difficulty"></param>
    /// <param name="time"></param>
    /// <returns>An integer with the score</returns>
    public int CalculateScore(int totalQuestions, int correctAnswers, string difficulty, double time)
    {
        GlobalConfig.Logger.Information($"QuizService: Calculating score for {totalQuestions} questions");
        int difficultyMultiplier = difficulty switch
        {
            "easy" => 1,
            "medium" => 3,
            "hard" => 5,
            _ => 1
        };
        double quotient = time / totalQuestions;
        int timeMultiplier = quotient < 5 ? 5 : quotient < 20 ? 3 : 1;

        double baseScore = ((double)correctAnswers / totalQuestions) * difficultyMultiplier * timeMultiplier;
        GlobalConfig.Logger.Information($"QuizService: Base score calculated: {baseScore} (correctAnswers: {correctAnswers}, totalQuestions: {totalQuestions}, difficultyMultiplier: {difficultyMultiplier}, timeMultiplier: {timeMultiplier})");
        return (int)Math.Round(baseScore * 100); // Multiply by 100 for a more meaningful score
    }
    
    
}