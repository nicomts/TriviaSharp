using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data;
using TriviaSharp.Data.Repositories;

namespace TriviaSharp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        // Configure SQLite with EF Core
        builder.Services.AddDbContext<TriviaDbContext>(options =>
            options.UseSqlite("Filename=triviasharp.db")); // You can change the path if needed

        // Register generic and specific repositories
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
        builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IQuizSessionRepository, QuizSessionRepository>();
        builder.Services.AddScoped<IQuestionSetRepository, QuestionSetRepository>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}