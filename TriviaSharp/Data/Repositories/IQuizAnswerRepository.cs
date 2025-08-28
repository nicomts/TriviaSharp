using TriviaSharp.Models;
namespace TriviaSharp.Data.Repositories;
public interface IQuizAnswerRepository: IGenericRepository<QuizAnswer>
{
    Task<IEnumerable<QuizAnswer>> GetAnswersByQuizSessionIdAsync(int quizSessionId);
}