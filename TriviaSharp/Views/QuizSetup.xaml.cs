using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.Models;
using TriviaSharp.Utils;
using TriviaSharp.Services;

namespace TriviaSharp.Views;

public partial class QuizSetup : ContentPage
{
    IEnumerable<Category> categories;
    Category selectedCategory;
    int selectedNumber = 10; // Default value for number of questions
    string selectedDifficulty = "";
    private QuizService quizService = new QuizService();
    
    
    public QuizSetup()
    {
        InitializeComponent();
        DifficultyCollectionView.ItemsSource = new[] { "Easy", "Medium", "Hard" };
    }
    
    private async void OnCategoriesButtonClicked(object sender, EventArgs e)
    {
        try
        {
            categories = await GlobalConfig.CategoryRepo.GetAllCategoriesAsync();
            CategoriesCollectionView.ItemsSource = categories.ToList();
            CategoriesCollectionView.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch categories: {ex.Message}", "OK");
        }
    }
    
    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        selectedCategory = CategoriesCollectionView.SelectedItem as Category;
        
    }
    
    private void OnNumberEntryChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(NumberEntry.Text, out int value) && value >= 10 && value <= 50)
        {
            selectedNumber = value;
            NumberErrorLabel.IsVisible = false;
        }
        else
        {
            NumberErrorLabel.Text = "Please enter a number between 10 and 50.";
            NumberErrorLabel.IsVisible = true;
        }
    }

    private void OnDifficultySelected(object sender, SelectionChangedEventArgs e)
    {
        selectedDifficulty = DifficultyCollectionView.SelectedItem as string;
        selectedDifficulty = selectedDifficulty.ToLower();

    }

    private async void OnStartQuizButtonClicked(object sender, EventArgs e)
    {
        var randomQuestions = await quizService.GetQuizAsync(selectedCategory, selectedDifficulty, selectedNumber, GlobalConfig.CurrentQuestionSet);
        if (randomQuestions.Count < 10)
        {
            await DisplayAlert("Error", "Not enough questions found for the selected criteria. Use an admin account to import at least 10 questions.", "OK");
            return;
        }
        
        // For each question in randomQuestions, console log the question text and answers. Also log the correct answer.
        foreach (var question in randomQuestions)
        {
            Console.WriteLine($"Question: {question.Text}");
            foreach (var answer in question.Answers)
            {
                Console.WriteLine($"Answer: {answer.Text} (Correct: {answer.IsCorrect})");
            }
        }
        
        
        
    }
    
    

}