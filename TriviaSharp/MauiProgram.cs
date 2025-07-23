using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data;
using TriviaSharp.Data.Repositories;
using TriviaSharp.OpenTDB;
using TriviaSharp.Services;
using TriviaSharp.Tests;

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
        
        // Run unit tests
        // UnitTestsRunner.RunAllTests();



#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}