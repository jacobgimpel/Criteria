using Criteria.Services;
using Criteria.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Criteria.Pages.MainApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnFindButtonClicked(object sender, EventArgs e)
    {

        var selectedGenresJson = Preferences.Get("SelectedGenres", "[]");
        var selectedMoviesJson = Preferences.Get("SelectedMovies", "[]");

        var selectedGenres = JsonConvert.DeserializeObject<List<string>>(selectedGenresJson) ?? new List<string>();
        var selectedMovies = JsonConvert.DeserializeObject<List<Movie>>(selectedMoviesJson) ?? new List<Movie>();

        if (selectedGenres.Count == 0 || selectedMovies.Count == 0)
        {
            await DisplayAlert("Error", "No movies or genres selected. Please complete the first boot process.", "OK");
            return;
        }

        var tmdbService = new TMDBService();
        var recommendedMovies = await tmdbService.GetRecommendationsAsync(selectedMovies, selectedGenres, 5);

     
        await Navigation.PushAsync(new RecommendationView(recommendedMovies));
    }

    private async void OnSavedFilmsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SavedFilms());
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}
