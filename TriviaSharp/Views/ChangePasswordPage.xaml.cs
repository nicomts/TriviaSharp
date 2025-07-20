using System;
using System.Linq;
using TriviaSharp.Utils;
using TriviaSharp.Models.Enums;

namespace TriviaSharp.Views;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (GlobalConfig.CurrentUser == null)
        {
            await DisplayAlert("Login Required", "You must be logged in to change your password.", "OK");
            await Navigation.PopAsync();
            return;
        }
        UsernameLabel.Text = $"User: {GlobalConfig.CurrentUser.Username}";
    }

    private void OnOldPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        OldPasswordErrorLabel.IsVisible = string.IsNullOrEmpty(e.NewTextValue);
        MessageLabel.Text = string.Empty;
        SuccessLabel.Text = string.Empty;
    }

    private void OnNewPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        NewPasswordErrorLabel.IsVisible = !IsValidPassword(e.NewTextValue);
        ConfirmNewPasswordErrorLabel.IsVisible = ConfirmNewPasswordEntry.Text != NewPasswordEntry.Text;
        MessageLabel.Text = string.Empty;
        SuccessLabel.Text = string.Empty;
    }

    private void OnConfirmNewPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        ConfirmNewPasswordErrorLabel.IsVisible = e.NewTextValue != NewPasswordEntry.Text;
        MessageLabel.Text = string.Empty;
        SuccessLabel.Text = string.Empty;
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        return password.Length >= 8 && password.Length <= 64;
    }

    private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
    {
        MessageLabel.Text = string.Empty;
        SuccessLabel.Text = string.Empty;

        var oldPassword = OldPasswordEntry.Text;
        var newPassword = NewPasswordEntry.Text;
        var confirmNewPassword = ConfirmNewPasswordEntry.Text;

        bool valid = true;
        if (string.IsNullOrEmpty(oldPassword))
        {
            OldPasswordErrorLabel.IsVisible = true;
            valid = false;
        }
        else
        {
            OldPasswordErrorLabel.IsVisible = false;
        }

        if (!IsValidPassword(newPassword))
        {
            NewPasswordErrorLabel.IsVisible = true;
            valid = false;
        }
        else
        {
            NewPasswordErrorLabel.IsVisible = false;
        }

        if (newPassword != confirmNewPassword)
        {
            ConfirmNewPasswordErrorLabel.IsVisible = true;
            valid = false;
        }
        else
        {
            ConfirmNewPasswordErrorLabel.IsVisible = false;
        }

        if (!valid)
        {
            MessageLabel.Text = "Please fix the errors above.";
            return;
        }

        var username = GlobalConfig.CurrentUser.Username;
        var status = await GlobalConfig.UserService.ChangePasswordAsync(username, oldPassword, newPassword);

        string alertMessage = status switch
        {
            ChangePasswordStatus.Success => "Password changed successfully.",
            ChangePasswordStatus.UserNotFound => "User not found. Please login again.",
            ChangePasswordStatus.IncorrectPassword => "Current password is incorrect.",
            _ => "Failed to change password."
        };

        await DisplayAlert("Change Password", alertMessage, "OK");

        if (status == ChangePasswordStatus.Success)
        {
            OldPasswordEntry.Text = "";
            NewPasswordEntry.Text = "";
            ConfirmNewPasswordEntry.Text = "";
            await Navigation.PopAsync();
        }
    }
}