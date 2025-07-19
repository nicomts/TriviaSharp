using TriviaSharp.Utils;

namespace TriviaSharp.Services;
using TriviaSharp.Models;

public class QuizService
{
    // This service controls the quiz logic, including starting a quiz, answering questions, and calculating scores.
    
    
    public async Task<List<Question>> GetQuizAsync(Category category, string difficulty, int numberOfQuestions, QuestionSet questionSet)
    {
        // Get random questions from the repository based on category, difficulty, amount and question set
        var randomQuestions = (await GlobalConfig.QuestionRepo.GetByCategoryAndDifficultyAsync(category, difficulty, questionSet))
            .OrderBy(q => Guid.NewGuid()).Take(numberOfQuestions).ToList();
        
        // DEBUG CODE
        // if (randomQuestions == null)
        // {
        //     Console.WriteLine("No questions found for the specified criteria.");
        // }
        //
        // Console.WriteLine($"Total Questions: {randomQuestions.Count}");
        // foreach (var question in randomQuestions)
        // {
        //     
        //     Console.WriteLine($"Question: {question.Text}");
        //     foreach (var answer in question.Answers)
        //     {
        //         Console.WriteLine($"Answer: {answer.Text} (Correct: {answer.IsCorrect})");
        //     }
        // }
        //
        
        
        // For each question in randomQuestions, random sort answers
        // foreach (var question in randomQuestions)
        // {
        //     question.Answers = question.Answers.OrderBy(a => Guid.NewGuid()).ToList();
        // }
        
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
        return randomQuestions;
        
    }
    
}