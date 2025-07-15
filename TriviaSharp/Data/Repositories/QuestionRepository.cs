namespace TriviaSharp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(TriviaDbContext context) : base(context) { }

    public async Task<IEnumerable<Question>> GetByCategoryAndDifficultyAsync(Category category, Difficulty difficulty)
    {
        return await _context.Questions
            .Where(q => q.Category == category && q.Difficulty == difficulty)
            .Include(q => q.Answers)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Question>> GetByTextAsync(string text)
    {
        return await _context.Questions
            .Where(q => q.Text.Contains(text))
            .Include(q => q.Answers)
            .ToListAsync();
    }
}
