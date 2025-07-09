using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;

namespace TriviaSharp.Data
{
    public class TriviaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizSession> QuizSessions { get; set; }
        public DbSet<QuizSessionAnswer> QuizSessionAnswers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "triviasharp.db3"
            );
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}