using TriviaSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace TriviaSharp.Data.Repositories;

public class QuizAnswerRepository : GenericRepository<QuizAnswer>, IQuizAnswerRepository
{
    public QuizAnswerRepository(TriviaDbContext context) : base(context) { }

    public async Task<IEnumerable<QuizAnswer>> GetAnswersByQuizSessionIdAsync(int quizSessionId)
    {
        return await _context.QuizAnswers
            .Where(answer => answer.QuizSessionId == quizSessionId)
            .ToListAsync();
    }



}
