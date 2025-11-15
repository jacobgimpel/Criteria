using Criteria.Models;
using System.Collections.Generic;

namespace Criteria.Pages.MainApp;

public partial class RecommendationView : ContentPage
{
    private readonly List<Movie> _recommendedMovies;

    public RecommendationView(List<Movie> recommendedMovies)
    {
        InitializeComponent();
        _recommendedMovies = recommendedMovies ?? new List<Movie>();

        RecommendationsCarousel.ItemsSource = _recommendedMovies;
    }

    private async void OnPosterTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image image && image.Parent is Grid parentGrid)
        {
            var overlay = parentGrid.FindByName<Grid>("Overlay");
            if (overlay == null) return;

            if (overlay.IsVisible)
            {
                await overlay.FadeTo(0, 250);
                overlay.IsVisible = false;
            }
            else
            {
                overlay.IsVisible = true;
                await overlay.FadeTo(1, 250);
            }
        }
    }

    private async void OnOverlayTapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid overlay)
        {
            await overlay.FadeTo(0, 250);
            overlay.IsVisible = false;
        }
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (RecommendationsCarousel.CurrentItem is Movie currentMovie)
        {
            bool alreadyAdded = Criteria.Models.SavedFilms.SavedMovies.Any(m => m.TMDBId == currentMovie.TMDBId);
            if (alreadyAdded)
            {
                DisplayAlert("Already Saved", $"{currentMovie.Title} is already saved.", "OK");
            }
            else
            {
                Criteria.Models.SavedFilms.AddMovie(currentMovie);
                DisplayAlert("Saved", $"{currentMovie.Title} has been added to your bookmarks.", "OK");
            }
        }
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSavedFilmsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SavedFilms());
    }
}
