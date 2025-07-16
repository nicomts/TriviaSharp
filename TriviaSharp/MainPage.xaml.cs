using TriviaSharp.Views;

using Microsoft.Extensions.DependencyInjection;
using TriviaSharp.Services;
using TriviaSharp.Views;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data.Repositories;

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
    TriviaDbContext dbContext = SetupDatabase();
    
    UserRepository userRepo = new Data.Repositories.UserRepository(dbContext);
    UserService userService = new Services.UserService(userRepo);
    
    
    
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
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

    // DEBUG CODE
    
    // User userRepository = new UserRepository(new TriviaDbContext());
    // UserService service = new UserService();
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(userService));
    }
}