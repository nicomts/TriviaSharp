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

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistrationPage());
    }
    private async void OnAdminPanelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminPanel());
    }
    
    private async void OnNewQuizButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QuizSetup());
    }
    
    private async void OnRankingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RankingPage());
    }
    private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
    {
        // if (GlobalConfig.CurrentUser == null)
        // {
        //     await DisplayAlert("Login Required", "You must be logged in to change your password.", "OK");
        //     return;
        // }
        await Navigation.PushAsync(new ChangePasswordPage());
    }
    private async void OnDeleteUserButtonClicked(object sender, EventArgs e)
    {
        // if (GlobalConfig.CurrentUser == null || GlobalConfig.CurrentUser.Role != UserRole.Admin)
        // {
        //     await DisplayAlert("Admin Required", "You must be an admin to delete users.", "OK");
        //     return;
        // }
        await Navigation.PushAsync(new DeleteUserPage());
    }
    
}