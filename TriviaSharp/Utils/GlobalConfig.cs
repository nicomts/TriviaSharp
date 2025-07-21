using TriviaSharp.Services;

namespace TriviaSharp.Utils;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models;
using Serilog;
using Serilog.Events;

public static class GlobalConfig
{
    // Logger initialization
    public static ILogger Logger = SetupLogger(); 
    
    // Database initialization and repositories
    public static TriviaDbContext DbContext = SetupDatabase();
    public static UserRepository UserRepo = new UserRepository(DbContext);
    public static QuestionRepository QuestionRepo = new QuestionRepository(DbContext);
    public static QuestionSetRepository QuestionSetRepo = new QuestionSetRepository(DbContext);
    public static AnswerRepository AnswerRepo = new AnswerRepository(DbContext);
    public static CategoryRepository CategoryRepo = new CategoryRepository(DbContext);
    public static QuizSessionRepository QuizSessionRepo = new QuizSessionRepository(DbContext);
    
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



    // Methods
    static GlobalConfig()
    {
        EnsureAdminUser();
    }
    
    public static ILogger SetupLogger()
    {
        var logFilePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config",
            "TriviaSharp",
            "logs",
            "triviasharp-.log"
        );
        return new LoggerConfiguration()
            .MinimumLevel.Debug() // Set the minimum level for all logs
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // Suppress verbose Microsoft logs
            .MinimumLevel.Override("System", LogEventLevel.Information)    // Suppress verbose System logs
            // If you are using SQLite/EF Core, you might want to override their log levels:
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.Data.Sqlite", LogEventLevel.Information)
            .Enrich.FromLogContext() // Enables structured logging context
            .WriteTo.File(
                path: logFilePath,
                rollingInterval: RollingInterval.Day, // Create a new log file daily
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB file size limit
                rollOnFileSizeLimit: true, // Roll to a new file when size limit is reached
                shared: true // Allow multiple processes to write to the same log file
            )
            .CreateLogger();
    }
    
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
    
    public static void EnsureAdminUser()
    {
        // Check if any user with Admin role exists
        var admins = UserRepo.FindAsync(u => u.Role == Models.Enums.UserRole.Admin).Result;
        if (!admins.Any())
        {
            UserService.RegisterAsync("admin", "triviasharp", Models.Enums.UserRole.Admin);
        }
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