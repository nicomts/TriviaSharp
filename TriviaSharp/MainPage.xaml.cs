using TriviaSharp.Views;

using Microsoft.Extensions.DependencyInjection;
using TriviaSharp.Services;
using TriviaSharp.Views;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Models.Enums;
using TriviaSharp.Models;
using TriviaSharp.Utils;


namespace TriviaSharp;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistrationPage());
    }
    private async void OnSettingsPageButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
    
    private async void OnNewQuizButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QuizSetup());
    }
    
    private async void OnRankingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RankingPage());
    }

    
}