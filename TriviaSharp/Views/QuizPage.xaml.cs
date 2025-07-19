using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TriviaSharp.Models;

namespace TriviaSharp.Views;

public partial class QuizPage : ContentPage
{
    private readonly List<Question> _questions;
    private int _currentIndex = 0;
    private int _correctCount = 0;
    private DateTime _startDate;
    private System.Timers.Timer _timer;
    private double _elapsedSeconds = 0;

    public QuizPage(List<Question> questions)
    {
        InitializeComponent();
        _questions = questions;
        _startDate = DateTime.UtcNow;
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
            TimerLabel.Text = $"Finished! Time: {_elapsedSeconds:F1} s";
            QuestionLabel.Text = "Quiz Complete!";
            AnswersLayout.Children.Clear();
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
            btn.Clicked += async (s, e) => await OnAnswerClicked(btn, answer.IsCorrect);
            AnswersLayout.Children.Add(btn);
        }
    }

    private async Task OnAnswerClicked(Button btn, bool isCorrect)
    {
        foreach (var child in AnswersLayout.Children.OfType<Button>())
            child.IsEnabled = false;

        if (isCorrect)
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

        await Task.Delay(1000); // Wait before next question
        _currentIndex++;
        ShowQuestion();
    }

    // Expose results if needed
    public DateTime QuizStartDate => _startDate;
    public double TimeTakenSeconds => _elapsedSeconds;
    public int CorrectAnswers => _correctCount;
}