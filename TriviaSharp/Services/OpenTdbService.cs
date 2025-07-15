namespace TriviaSharp.Services;

using System;
using System.Threading.Tasks;
using TriviaSharp.Data.Repositories;
using TriviaSharp.OpenTDB;
using TriviaSharp.Models;

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

            _categoryRepo.SaveChangesAsync();
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

            // Map API question to local Question model
            var question = new Question
            {
                Text = apiQ.QuestionText,
                Category = category,
                QuestionSet = questionSet
            };
            await _questionRepo.AddAsync(question);

            // Map API answers to local Answer model
            var correctAnswer = new Answer
            {
                Text = apiQ.CorrectAnswer,
                IsCorrect = true,
                Question = question
            };
            await _answerRepo.AddAsync(correctAnswer);

            foreach (var incorrectText in apiQ.IncorrectAnswers)
            {
                var incorrectAnswer = new Answer
                {
                    Text = incorrectText,
                    IsCorrect = false,
                    Question = question
                };
                await _answerRepo.AddAsync(incorrectAnswer);
                
            }
            // Save changes after processing each question
            await _questionRepo.SaveChangesAsync();
            await _answerRepo.SaveChangesAsync();
            
        }
    }
}