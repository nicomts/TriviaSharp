namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;

public interface IQuizSessionRepository : IGenericRepository<QuizSession>
{
    Task<IEnumerable<QuizSession>> GetTop20ByScoreAsync();
}
