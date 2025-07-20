using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Utils;
using TriviaSharp.Models.Enums;

namespace TriviaSharp.Views;

public partial class DeleteUserPage : ContentPage
{
    public DeleteUserPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Check if current user is admin
        if (GlobalConfig.CurrentUser == null || GlobalConfig.CurrentUser.Role != UserRole.Admin)
        {
            await DisplayAlert("Admin Required", "You must be an admin to delete users.", "OK");
            await Navigation.PopAsync();
        }
    }

    private async void OnDeleteUserClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text?.Trim();
        if (string.IsNullOrEmpty(username))
        {
            await DisplayAlert("Input Required", "Please enter a username.", "OK");
            return;
        }

        if (username == GlobalConfig.CurrentUser?.Username)
        {
            await DisplayAlert("Error", "You cannot delete your own account while logged in.", "OK");
            return;
        }

        var user = await GlobalConfig.UserRepo.GetByUsernameAsync(username);
        if (user == null)
        {
            await DisplayAlert("User Not Found", $"No user found with username '{username}'.", "OK");
            return;
        }

        bool confirm = await DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete the user '{username}'?",
            "Yes", "No"
        );

        if (!confirm)
            return;

        await GlobalConfig.UserRepo.DeleteAsync(user);
        await GlobalConfig.UserRepo.SaveChangesAsync();

        await DisplayAlert("Success", $"User '{username}' has been deleted.", "OK");
        await Navigation.PopAsync();
    }
}