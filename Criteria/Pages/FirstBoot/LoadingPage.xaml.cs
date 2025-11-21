using Criteria.Services;
using Criteria.Models;
using Criteria.Utilities;
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
        _ = LoadRecommendationsAsync();
        this.Appearing += (s, e) => Services.OrientationService.LockPortrait();
        this.Disappearing += (s, e) => Services.OrientationService.UnlockOrientation();
    }

    private void SaveUserData()
    {
        Preferences.Set(SetPreferences.SelectionsCompleted, true);
        Preferences.Set(SetPreferences.SelectedGenres, JsonConvert.SerializeObject(_selectedGenres));
        Preferences.Set(SetPreferences.SelectedMovies, JsonConvert.SerializeObject(_selectedMovies));
    }

    private async Task LoadRecommendationsAsync()
    {
        try
        {
            var tmdbService = new Criteria.Services.TMDBService();


            var recommendedMovies = await tmdbService.GetRecommendationsAsync(_selectedMovies, _selectedGenres, 5);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current.MainPage = new NavigationPage(new Pages.MainApp.RecommendationView(recommendedMovies));
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load recommendations: {ex.Message}", "OK");
        }
    }
}
