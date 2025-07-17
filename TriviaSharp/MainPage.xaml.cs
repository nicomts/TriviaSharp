using TriviaSharp.Views;

using Microsoft.Extensions.DependencyInjection;
using TriviaSharp.Services;
using TriviaSharp.Views;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models.Enums;
using TriviaSharp.Models;

//DEBUG CODE
// using TriviaSharp.Services;
// using TriviaSharp.Data.Repositories;
// using TriviaSharp.Models;
// using TriviaSharp.LocalTest;


namespace TriviaSharp;

public partial class MainPage : ContentPage
{
    // Initial setup
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
    TriviaDbContext dbContext;
    UserRepository userRepo;
    UserService userService;
    
    
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
        
        // Initialize in constructor
        dbContext = SetupDatabase();
        userRepo = new Data.Repositories.UserRepository(dbContext);
        userService = new Services.UserService(userRepo);
    }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(userService));
    }
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistrationPage(userService, UserSessionService.Instance.CurrentUser));
    }
}