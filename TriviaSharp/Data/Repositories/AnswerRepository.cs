namespace TriviaSharp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;

public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
{
    public AnswerRepository(TriviaDbContext context) : base(context) { }

    public async Task<IEnumerable<Answer>> GetCorrectAnswersByQuestionIdAsync(int questionId)
    {
        return await _context.Answers
            .Where(a => a.QuestionId == questionId && a.IsCorrect)
            .ToListAsync();
    }
}
