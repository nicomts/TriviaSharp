using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Services;

namespace TriviaSharp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }
    
    private async void OnOpenTdbButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OpenTdbPanel());
    }

    private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePasswordPage());
    }
    private async void OnDeleteUserButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeleteUserPage());
    }
}
