using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Services;

namespace TriviaSharp.Views;

public partial class AdminPanel : ContentPage
{
    public AdminPanel()
    {
        InitializeComponent();
    }
    
    private async void OnOpenTdbButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OpenTdbPanel());
    }
}