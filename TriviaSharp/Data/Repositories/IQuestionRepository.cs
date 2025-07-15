namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<IEnumerable<Question>> GetByCategoryAndDifficultyAsync(Category category, Difficulty difficulty);
    Task<IEnumerable<Question>> GetByTextAsync(string text);
}
