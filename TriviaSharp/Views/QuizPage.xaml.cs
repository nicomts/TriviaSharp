using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TriviaSharp.Models;
using TriviaSharp.Services;
using TriviaSharp.Utils;

namespace TriviaSharp.Views;

public partial class QuizPage : ContentPage
{
    private readonly List<Question> _questions;
    private int _currentIndex = 0;
    private int _correctCount = 0;

    private System.Timers.Timer _timer;
    private double _elapsedSeconds = 0;

    private QuizService _quizService;
    private QuizSession _quizSession;


    public QuizPage(List<Question> questions, string difficulty)
    {
        InitializeComponent();
        _questions = questions;
        _quizService = new QuizService();
        _quizSession = _quizService.StartSession(GlobalConfig.CurrentUser, difficulty, DateTime.UtcNow);

        GlobalConfig.Logger.Information($"QuizPage: Starting quiz with {_questions.Count} questions at difficulty '{difficulty}'");
        StartTimer();
        ShowQuestion();
    }

    private void StartTimer()
    {
        _timer = new System.Timers.Timer(100);
        _timer.Elapsed += (s, e) =>
        {
            _elapsedSeconds += 0.1;
            var ts = TimeSpan.FromSeconds(_elapsedSeconds);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TimerLabel.Text = $"Time: {ts.Minutes:D2}:{ts.Seconds:D2}";
            });
        };
        _timer.Start();
    }

    private void ShowQuestion()
    {
        if (_currentIndex >= _questions.Count)
        {
            _timer.Stop();
            _quizService.EndSessionAsync(_quizSession, _elapsedSeconds);
            TimerLabel.Text = $"Finished! Time: {_quizSession.TimeTakenSeconds:F1} s";
            QuestionLabel.Text = "Quiz Complete!";
            AnswersLayout.Children.Clear();
            ScoreLabel.Text = $"Score: {_quizSession.Score}";
            GlobalConfig.Logger.Information($"QuizPage: Quiz completed with score {_quizSession.Score} at difficulty '{_quizSession.Difficulty}'");
            return;
        }

        var q = _questions[_currentIndex];
        QuestionLabel.Text = q.Text;
        CounterLabel.Text = $"Question {_currentIndex + 1} of {_questions.Count}";
        ScoreLabel.Text = $"Correct: {_correctCount}";

        AnswersLayout.Children.Clear();
        foreach (var answer in q.Answers)
        {
            var btn = new Button
            {
                Text = answer.Text,
                BackgroundColor = Colors.MediumPurple
            };
            btn.Clicked += async (s, e) => await OnAnswerClicked(btn, answer);
            AnswersLayout.Children.Add(btn);
        }
    }

    private async Task OnAnswerClicked(Button btn, Answer answer)
    {
        foreach (var child in AnswersLayout.Children.OfType<Button>())
            child.IsEnabled = false;

        if (answer.IsCorrect)
        {
            btn.BackgroundColor = Colors.DarkGreen;
            _correctCount++;
        }
        else
        {
            btn.BackgroundColor = Colors.DarkRed;
            // Optionally highlight the correct answer
            foreach (var child in AnswersLayout.Children.OfType<Button>())
            {
                if (_questions[_currentIndex].Answers
                    .First(a => a.Text == child.Text).IsCorrect)
                    child.BackgroundColor = Colors.DarkGreen;
            }
        }
        ScoreLabel.Text = $"Correct: {_correctCount}";

        _quizService.AddAnswer(_quizSession, _questions[_currentIndex], answer);
        await Task.Delay(1000); // Wait before next question
        _currentIndex++;
        ShowQuestion();
    }

    // Expose results if needed
    // public DateTime QuizStartDate => _startDate;
    public double TimeTakenSeconds => _elapsedSeconds;
    public int CorrectAnswers => _correctCount;
}