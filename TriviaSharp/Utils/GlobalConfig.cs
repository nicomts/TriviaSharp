using TriviaSharp.Services;

namespace TriviaSharp.Utils;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models;

public static class GlobalConfig
{
    // Database initialization and repositories
    public static TriviaDbContext DbContext = SetupDatabase();
    public static UserRepository UserRepo = new UserRepository(DbContext);
    public static QuestionRepository QuestionRepo = new QuestionRepository(DbContext);
    public static QuestionSetRepository QuestionSetRepo = new QuestionSetRepository(DbContext);
    public static AnswerRepository AnswerRepo = new AnswerRepository(DbContext);
    public static CategoryRepository CategoryRepo = new CategoryRepository(DbContext);
    
    // Services
    public static UserService UserService = new UserService(UserRepo);
    public static OpenTdbService OpenTdbService = new OpenTdbService(
        QuestionRepo,
        AnswerRepo,
        CategoryRepo,
        QuestionSetRepo
    );
    
    // Session management
    public static User? CurrentUser = null;
    public static QuestionSet CurrentQuestionSet = SetupQuestionSet();

    
    
    
    
    public static TriviaDbContext SetupDatabase()
    {
        // Locate database file at ~/.config/TriviaSharp/triviasharp.db
        var dbPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config",
            "TriviaSharp",
            "triviasharp.db"
        );
        
        var dbDir = System.IO.Path.GetDirectoryName(dbPath);
        if (!System.IO.Directory.Exists(dbDir))
        {
            System.IO.Directory.CreateDirectory(dbDir);
        }
        var options = new DbContextOptionsBuilder<TriviaDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;
        var context = new TriviaDbContext(options);
        // Ensure database and tables are created
        context.Database.EnsureCreated();
        return context;
    }
    
    // Method to setup a default QuestionSet. It retrieves the question set with the name "OpenTDB" from the database or creates a new one if it doesn't exist.
    public static QuestionSet SetupQuestionSet()
    {
        var questionSet = QuestionSetRepo.GetByNameAsync("OpenTDB").Result;
        if (questionSet == null)
        {
            questionSet = new QuestionSet
            {
                Name = "OpenTDB",
            };
            QuestionSetRepo.AddAsync(questionSet);
            QuestionSetRepo.SaveChangesAsync();
        }
        return questionSet;
    }
    
}