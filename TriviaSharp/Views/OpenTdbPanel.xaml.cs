using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaSharp.OpenTDB;
using TriviaSharp.Models.Enums;
using TriviaSharp.Services;
using TriviaSharp.Utils;

namespace TriviaSharp.Views;

public partial class OpenTdbPanel : ContentPage
{
    ApiCategory[] categories;
    ApiCategory selectedCategory;
    int selectedNumber = 10; // Default value for number of questions
    string type = "";
    string selectedDifficulty = "";
    string sessionToken = "";
    
    private class TypeOption
    {
        public string Display { get; set; }
        public string Value { get; set; }
    }
    
    public OpenTdbPanel()
    {
        InitializeComponent();
        DifficultyCollectionView.ItemsSource = new[] { "Easy", "Medium", "Hard" };
        TypeCollectionView.ItemsSource = new List<TypeOption>
        {
            new TypeOption { Display = "Multiple choice", Value = "multiple" },
            new TypeOption { Display = "True or False", Value = "boolean" }
        };
        RequestWarning.Text = "Warning: You need to wait 5 seconds before making another request to the OpenTDB API. This is to prevent hitting the rate limit of the API. If you hit the rate limit, you will receive an error message.";
    }
   
    private async void OnSessionTokenButtonClicked(object sender, EventArgs e)
    {
        try
        {
            sessionToken = await TriviaSharp.OpenTDB.OpenTdbFetcher.GetSessionTokenAsync();
            SessionTokenLabel.Text = $"Session Token: {sessionToken}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch session token: {ex.Message}", "OK");
        }
    }
    private async void OnCategoriesButtonClicked(object sender, EventArgs e)
    {
        try
        {
            categories = await TriviaSharp.OpenTDB.OpenTdbFetcher.GetTriviaCategoriesAsync();
            CategoriesCollectionView.ItemsSource = categories;
            CategoriesCollectionView.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch categories: {ex.Message}", "OK");
        }
    }

    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        selectedCategory = e.CurrentSelection.FirstOrDefault() as ApiCategory;
    }
    
    private void OnNumberEntryChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(NumberEntry.Text, out int value) && value >= 1 && value <= 50)
        {
            selectedNumber = value;
            NumberErrorLabel.IsVisible = false;
        }
        else
        {
            NumberErrorLabel.Text = "Please enter a number between 1 and 50.";
            NumberErrorLabel.IsVisible = true;
        }
    }
    
    private void OnDifficultySelected(object sender, SelectionChangedEventArgs e)
    {
        selectedDifficulty = DifficultyCollectionView.SelectedItem as string;
        selectedDifficulty = selectedDifficulty.ToLower();
        // DEBUG CODE
        // Console.WriteLine($"Selected Difficulty: {selectedDifficulty}");
        
        
    }
    private void OnTypeSelected(object sender, SelectionChangedEventArgs e)
    {
        if (TypeCollectionView.SelectedItem is TypeOption option)
            type = option.Value;
    }
    
    private async void OnGetQuestionsButtonClicked(object sender, EventArgs e)
    {
        if (selectedCategory == null)
        {
            DisplayAlert("Error", "Please select a category.", "OK");
            return;
        }
        
        if (string.IsNullOrEmpty(type))
        {
            DisplayAlert("Error", "Please select a question type.", "OK");
            return;
        }
        
        try
        {
            var questions = await OpenTdbFetcher.GetTriviaQuestionsAsync(
                amount: selectedNumber,
                category: selectedCategory.Id,
                difficulty: selectedDifficulty,
                type: type,
                sessionToken: sessionToken
                // selectedNumber, selectedCategory.Id, selectedDifficulty, type, sessionToken
            );
            GlobalConfig.OpenTdbService.ImportApiQuestions(questions);
            await DisplayAlert("Success", $"{questions.Length} questions fetched successfully!", "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch questions: {ex.Message}", "OK");
        }
        
    }
    
}