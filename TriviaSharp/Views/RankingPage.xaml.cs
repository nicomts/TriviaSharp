using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Models;
using TriviaSharp.Utils;

namespace TriviaSharp.Views;

public partial class RankingPage : ContentPage
{
    public List<QuizSession> QuizSessions { get; set; } = new();

    public RankingPage()
    {
        InitializeComponent();
        LoadRanking();
        BindingContext = this;
    }

    private async void LoadRanking()
    {
        var sessions = await GlobalConfig.QuizSessionRepo.GetTop20ByScoreAsync();
        QuizSessions = sessions.ToList();
        RankingList.ItemsSource = QuizSessions;
        GlobalConfig.Logger.Information("RankingPage: Loaded top 20 quiz sessions for ranking display");
    }
}