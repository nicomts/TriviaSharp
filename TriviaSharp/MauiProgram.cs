using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data;
using TriviaSharp.Data.Repositories;
using TriviaSharp.OpenTDB;
using TriviaSharp.Services;

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
        




#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}