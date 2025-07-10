namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;

public interface IQuestionSetRepository : IGenericRepository<QuestionSet>
{
    Task<QuestionSet?> GetByNameAsync(string name);
}
