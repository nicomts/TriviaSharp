using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Models.Enums;
using TriviaSharp.Services;
using TriviaSharp.Models;

namespace TriviaSharp.Views;
using TriviaSharp.Utils;

public partial class RegistrationPage : ContentPage
{
    
    public RegistrationPage()
    {
        InitializeComponent();

        if (GlobalConfig.CurrentUser == null)
        {
            AdminRoleCheckBox.IsEnabled = false;
            AdminRoleCheckBox.IsChecked = false; // Disable admin role selection for non-admin users
        }
        else if (GlobalConfig.CurrentUser.Role == UserRole.Admin)
        {
            AdminRoleCheckBox.IsEnabled = true;
        } 
        else
        {
            AdminRoleCheckBox.IsEnabled = false;
            AdminRoleCheckBox.IsChecked = false; // Disable admin role selection for non-admin users
        }
        
        
    }
    
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        UsernameErrorLabel.IsVisible = !IsValidUsername(e.NewTextValue);
        MessageLabel.Text = string.Empty;
    }

    private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        PasswordErrorLabel.IsVisible = !IsValidPassword(e.NewTextValue);
        ConfirmPasswordErrorLabel.IsVisible = ConfirmPasswordEntry.Text != PasswordEntry.Text;
    }

    private void OnConfirmPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        ConfirmPasswordErrorLabel.IsVisible = e.NewTextValue != PasswordEntry.Text;
    }

    private bool IsValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return false;
        if (username.Length < 4 || username.Length > 20) return false;
        return username.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        return password.Length >= 8 && password.Length <= 64;
    }

    private void OnAdminRoleCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // placeholder for any additional logic when the admin role checkbox is checked or unchecked
    }
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;
        var confirmPassword = ConfirmPasswordEntry.Text;
        var role = UserRole.Regular;
        if (AdminRoleCheckBox.IsChecked)
        {
            role = UserRole.Admin;
        }

        if (!IsValidUsername(username))
        {
            MessageLabel.Text = "Invalid username.";
            return;
        }

        if (!IsValidPassword(password))
        {
            MessageLabel.Text = "Invalid password.";
            return;
        }

        if (password != confirmPassword)
        {
            MessageLabel.Text = "Passwords do not match.";
            return;
        }

        // Registration logic here (e.g., call a UserService.RegisterAsync)
        var registrationSuccess = GlobalConfig.UserService.RegisterAsync(username, password, role);
        if (!registrationSuccess.Result)
        {
            MessageLabel.Text = "Registration failed. User already exists.";
            return;
        }
        else
        {
            // SuccessLabel.Text = "Registration successful!";
            await DisplayAlert("Registration Successful", $"You can now log in with the {username} account.", "OK");
            await Navigation.PopAsync();
        }
        
    }




}