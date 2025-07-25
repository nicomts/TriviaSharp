namespace TriviaSharp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(TriviaDbContext context) : base(context) { }

    public async Task<IEnumerable<Question>> GetByCategoryAndDifficultyAsync(Category category, string difficulty, QuestionSet set)
    {
        return await _context.Questions
            .Where(q => q.Category == category && q.Difficulty == difficulty && q.QuestionSetId == set.Id)
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
    
    public async Task<IEnumerable<Question>> GetByTextCategoryAndDifficultyAsync(string text, Category category, string difficulty, QuestionSet set)
    {
        return await _context.Questions
            .Where(q => q.Text.Contains(text) && q.Category == category && q.Difficulty == difficulty && q.QuestionSetId == set.Id)
            .Include(q => q.Answers)
            .ToListAsync();
    }
}
