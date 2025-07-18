namespace TriviaSharp.Services;

using System;
using System.Threading.Tasks;
using TriviaSharp.Data.Repositories;
using TriviaSharp.OpenTDB;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public class OpenTdbService
{
    private readonly IQuestionRepository _questionRepo;
    private readonly IAnswerRepository _answerRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IQuestionSetRepository _questionSetRepo;

    public OpenTdbService(
        IQuestionRepository questionRepo,
        IAnswerRepository answerRepo,
        ICategoryRepository categoryRepo,
        IQuestionSetRepository questionSetRepo)
    {
        _questionRepo = questionRepo;
        _answerRepo = answerRepo;
        _categoryRepo = categoryRepo;
        _questionSetRepo = questionSetRepo;
    }

    public static Category ToCategory(ApiCategory apiCategory)
    {
        return new Category
        {
            Id = apiCategory.Id,
            Name = apiCategory.Name
        };
    }
    
    public async Task ImportCategories()
    {
        var categories = await OpenTdbFetcher.GetTriviaCategoriesAsync();
        foreach (var apiCategory in categories)
        {
            var category = ToCategory(apiCategory);
            var existingCategory = await _categoryRepo.GetByNameAsync(category.Name);
            if (existingCategory == null)
            {
                await _categoryRepo.AddAsync(category);
            }

            await _categoryRepo.SaveChangesAsync();
        }
    }

    public async Task ImportApiQuestions(ApiQuestion[] apiQuestions)
    {
        // Ensure "OpenTDB" QuestionSet exists
        var questionSet = await _questionSetRepo.GetByNameAsync("OpenTDB");
        if (questionSet == null)
        {
            questionSet = new QuestionSet { Name = "OpenTDB" };
            await _questionSetRepo.AddAsync(questionSet);
        }
        await _questionSetRepo.SaveChangesAsync();

        foreach (var apiQ in apiQuestions)
        {
            // Ensure category exists
            var category = await _categoryRepo.GetByNameAsync(apiQ.Category)
                ?? new Category { Name = apiQ.Category };
            if (category.Id == 0)
                await _categoryRepo.AddAsync(category);
            
           
            

            
            
            // Ensure question does not already exist
            // var existingQuestion = (await _questionRepo.GetByTextAsync(question.Text)).FirstOrDefault();
            Question existingQuestion = null;
            var existingQuestionQuery = await _questionRepo.GetByTextCategoryAndDifficultyAsync(
                apiQ.QuestionText,
                category,
                apiQ.Difficulty,
                questionSet);
            existingQuestion = existingQuestionQuery.FirstOrDefault();
            // DEBUG CODE
            // Console.WriteLine($"Checking question: {apiQ.QuestionText} in category: {category.Name} with difficulty: {apiQ.Difficulty}");
            // Console.WriteLine($"Existing question data: {existingQuestion?.Text ?? "None"}");
            
            if (existingQuestion != null)
            {
                continue; // Skip to the next apiQ if question already exists
            }
            else
            {
                // Map API question to local Question model
                var question = new Question
                {
                    Text = apiQ.QuestionText,
                    Difficulty = apiQ.Difficulty,
                    Category = category,
                    QuestionSet = questionSet
                };

                // Map API answers to local Answer model
                var correctAnswer = new Answer
                {
                    Text = apiQ.CorrectAnswer,
                    IsCorrect = true,
                    Question = question
                };
                await _answerRepo.AddAsync(correctAnswer);
                await _answerRepo.SaveChangesAsync();


                foreach (var incorrectText in apiQ.IncorrectAnswers)
                {
                    var incorrectAnswer = new Answer
                    {
                        Text = incorrectText,
                        IsCorrect = false,
                        Question = question
                    };
                    await _answerRepo.AddAsync(incorrectAnswer);
                    await _answerRepo.SaveChangesAsync();

                
                }
                // Save changes after processing each question
                await _questionRepo.SaveChangesAsync();
                await _answerRepo.SaveChangesAsync();
            }
            
            
            
            
            
            
            
            
            // if (existingQuestion == null)
            // {
            //     // Add question to the repository
            //     await _questionRepo.AddAsync(question);
            //     await _questionRepo.SaveChangesAsync();
            // }
            // else
            // {
            //     continue; // Skip to the next apiQ
            // }
            //
            
            
        }
    }
}