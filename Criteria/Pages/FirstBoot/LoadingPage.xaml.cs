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
        _=LoadRecommendationsAsync();
    }

    private void SaveUserData()
    {
        Preferences.Set("HasCompletedFirstBoot", true);
        Preferences.Set("SelectedGenres", JsonConvert.SerializeObject(_selectedGenres));
        Preferences.Set("SelectedMovies", JsonConvert.SerializeObject(_selectedMovies));
    }

    private async Task LoadRecommendationsAsync()
    {
        var tmdbService = new Services.TMDBService();
        var recommendedMovies = await tmdbService.GetRecommendationsAsync(_selectedMovies, _selectedGenres, 5);
        Application.Current.MainPage = new NavigationPage(new Pages.MainApp.RecommendationView(recommendedMovies));
    }
}
