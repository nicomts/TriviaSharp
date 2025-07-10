namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;

public interface IAnswerRepository : IGenericRepository<Answer>
{
    Task<IEnumerable<Answer>> GetCorrectAnswersByQuestionIdAsync(int questionId);
}
