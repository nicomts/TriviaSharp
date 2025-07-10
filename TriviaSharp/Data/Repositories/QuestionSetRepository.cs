namespace TriviaSharp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;

public class QuestionSetRepository : GenericRepository<QuestionSet>, IQuestionSetRepository
{
    public QuestionSetRepository(TriviaDbContext context) : base(context) { }

    public async Task<QuestionSet?> GetByNameAsync(string name)
    {
        return await _context.QuestionSets.FirstOrDefaultAsync(q => q.Name == name);
    }
}
