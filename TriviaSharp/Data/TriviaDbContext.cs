using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;

public class TriviaDbContext : DbContext
{
    public TriviaDbContext(DbContextOptions<TriviaDbContext> options) : base(options) { }

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<QuizSession> QuizSessions => Set<QuizSession>();
    public DbSet<QuestionSet> QuestionSets => Set<QuestionSet>();
}