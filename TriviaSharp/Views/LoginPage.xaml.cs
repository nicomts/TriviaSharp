using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Services;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;
using TriviaSharp.Models.Enums;
using TriviaSharp.Utils;

namespace TriviaSharp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        var username = e.NewTextValue;
        UsernameErrorLabel.IsVisible = !IsValidUsername(username);
    }

    private bool IsValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return false;
        if (username.Length < 4 || username.Length > 20) return false;
        return username.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
    }
    
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;
        var loginResult = await GlobalConfig.UserService.LoginAsync(username, password);
        switch (loginResult.Status) { 
            case LoginStatus.Success:
                // UserSessionService.Instance.CurrentUser = loginResult.User; // Set the current user
                GlobalConfig.CurrentUser = loginResult.User;
                await DisplayAlert("Login Successful", $"Welcome, {GlobalConfig.CurrentUser.Username}!", "OK");
                await Navigation.PopAsync(); // Navigate back to the previous page
                break;
            case LoginStatus.UserNotFound:
                await DisplayAlert("Login Failed", "User not found. Please check your username.", "OK");
                break;
            case LoginStatus.IncorrectPassword:
                await DisplayAlert("Login Failed", "Incorrect password. Please try again.", "OK");
                break;
        }
        

    }
}