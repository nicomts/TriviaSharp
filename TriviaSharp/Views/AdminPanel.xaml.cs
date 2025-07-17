using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Services;

namespace TriviaSharp.Views;

public partial class AdminPanel : ContentPage
{
    private readonly OpenTdbService _openTdbService;
    public AdminPanel(OpenTdbService openTdbService)
    {
        InitializeComponent();
        _openTdbService = openTdbService;
    }
    
    private async void OnOpenTdbButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OpenTdbPanel(_openTdbService));
    }
}