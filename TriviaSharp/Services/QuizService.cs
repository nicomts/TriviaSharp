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

    /// <summary>
    /// Starts a new quiz session.
    /// </summary>
    /// <param name="user">Current logged in user</param>
    /// <param name="difficulty">Selected difficulty level of the quiz</param>
    /// <param name="date">Date when the quiz is started</param>
    /// <returns></returns>
    public QuizSession StartSession(User user, string difficulty, DateTime date)
    {
        var quizSession = new QuizSession
        {
            User = user,
            Difficulty = difficulty,
            Date = date

        };
        GlobalConfig.QuizSessionRepo.AddAsync(quizSession);
        GlobalConfig.QuizSessionRepo.SaveChangesAsync();
        GlobalConfig.Logger.Information($"QuizService: Quiz session started with ID {quizSession.Id} for user {user.Username}");
        return quizSession;
    }

    /// <summary>
    /// Adds a user's answer to the quiz session.
    /// </summary>
    /// <param name="quizSession">Current quiz session</param>
    /// <param name="question">Question being answered</param>
    /// <param name="selectedAnswer">User's selected answer</param>
    public async void AddAnswer(QuizSession quizSession, Question question, Answer selectedAnswer)
    {
        var quizAnswer = new QuizAnswer
        {
            QuizSessionId = quizSession.Id,
            QuestionId = question.Id,
            SelectedOptionId = selectedAnswer.Id
        };
        quizSession.Answers.Add(quizAnswer);
        GlobalConfig.QuizSessionRepo.Update(quizSession);
        await GlobalConfig.QuizSessionRepo.SaveChangesAsync();
        // GlobalConfig.QuizAnswerRepo.AddAsync(quizAnswer);
        // GlobalConfig.QuizAnswerRepo.SaveChangesAsync();
        GlobalConfig.Logger.Information($"QuizService: Added answer with ID {quizAnswer.Id} to quiz session ID {quizSession.Id} for question ID {question.Id}");
    }

    /// <summary>
    /// Ends the quiz session and calculates the final score.
    /// </summary>
    /// <param name="quizSession">Current quiz session</param>
    /// <param name="timeTakenSeconds">Time taken to complete the quiz</param>
    /// <returns></returns>
    public async Task EndSessionAsync(QuizSession quizSession, double timeTakenSeconds)
    {
        int totalQuestions = quizSession.Answers.Count;
        quizSession.TimeTakenSeconds = timeTakenSeconds;

        // Get the count of QuizSession Answers where SelectedOptionId matches an Answer Id where IsCorrect == True
        int correctAnswers = 0;
        foreach (var quizAnswer in quizSession.Answers)
        {
            var answer = GlobalConfig.AnswerRepo.GetByIdAsync(quizAnswer.SelectedOptionId).Result;
            if (answer != null && answer.IsCorrect)
            {
                correctAnswers++;
            }
        }

        quizSession.Score = CalculateScore(totalQuestions, correctAnswers, quizSession.Difficulty, quizSession.TimeTakenSeconds);
        GlobalConfig.QuizSessionRepo.Update(quizSession);
        await GlobalConfig.QuizSessionRepo.SaveChangesAsync();
        GlobalConfig.Logger.Information($"QuizService: Ended quiz session ID {quizSession.Id} with score {quizSession.Score} and time taken {quizSession.TimeTakenSeconds} seconds");
    }
}