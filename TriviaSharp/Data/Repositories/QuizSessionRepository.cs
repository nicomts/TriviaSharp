namespace TriviaSharp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;

public class QuizSessionRepository : GenericRepository<QuizSession>, IQuizSessionRepository
{
    public QuizSessionRepository(TriviaDbContext context) : base(context) { }

    public async Task<IEnumerable<QuizSession>> GetTop20ByScoreAsync()
    {
        return await _context.QuizSessions
            .Include(s => s.User)
            .OrderByDescending(s => s.Score)
            .Take(20)
            .ToListAsync();
    }
    public async Task Update(QuizSession quizSession)
    {
        _context.QuizSessions.Update(quizSession);
        await _context.SaveChangesAsync();
    }
}
