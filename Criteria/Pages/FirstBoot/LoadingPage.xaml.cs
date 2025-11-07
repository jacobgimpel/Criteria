using Criteria.Models;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;

namespace Criteria.Pages.FirstBoot;

public partial class LoadingPage : ContentPage
{
    private readonly List<Movie> _selectedMovies;
    private readonly List<string> _selectedGenres;

    public LoadingPage(List<string> selectedGenres, List<Movie> selectedMovies)
    {
        InitializeComponent();
        _selectedGenres = selectedGenres;
        _selectedMovies = selectedMovies;

        SaveUserData();
        LoadRecommendationsAsync();
    }

    private void SaveUserData()
    {
        Preferences.Set("HasCompletedFirstBoot", true);
        Preferences.Set("SelectedGenres", JsonConvert.SerializeObject(_selectedGenres));
        Preferences.Set("SelectedMovies", JsonConvert.SerializeObject(_selectedMovies));
    }

    private async void LoadRecommendationsAsync()
    {
        await Task.Delay(5000);

        var recommendedMovies = _selectedMovies; // Placeholder for actual recommendation logic

        Application.Current.MainPage = new NavigationPage(new Pages.MainApp.RecommendationView(recommendedMovies));
    }
}
