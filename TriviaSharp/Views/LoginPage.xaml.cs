using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Data.Repositories;
using TriviaSharp.Services;
using Microsoft.EntityFrameworkCore;

namespace TriviaSharp.Views;

public partial class LoginPage : ContentPage
{
    private UserService _userService;
    public LoginPage(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
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
        var loginResult = await _userService.LoginAsync(username, password);
        MessageLabel.Text = loginResult.Status.ToString();

    }
}