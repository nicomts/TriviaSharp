namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<IEnumerable<Question>> GetByCategoryAndDifficultyAsync(Category category, string difficulty, QuestionSet set);
    Task<IEnumerable<Question>> GetByTextAsync(string text);
    Task<IEnumerable<Question>> GetByTextCategoryAndDifficultyAsync(string text, Category category, string difficulty, QuestionSet set);
}
