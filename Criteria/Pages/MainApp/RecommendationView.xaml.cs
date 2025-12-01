using Criteria.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Criteria.Pages.MainApp;

public partial class RecommendationView : ContentPage
{
    private readonly List<Movie> _recommendedMovies;

    public RecommendationView(List<Movie> recommendedMovies)
    {
        InitializeComponent();
        _recommendedMovies = recommendedMovies ?? new List<Movie>();

        PortraitCarousel.ItemsSource = _recommendedMovies;
        LandscapeCarousel.ItemsSource = _recommendedMovies;
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

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        Movie currentMovie = null;

        if (LandscapeGrid.IsVisible && LandscapeCarousel.CurrentItem is Movie landscapeMovie)
        {
            currentMovie = landscapeMovie;
        }
        else if (PortraitCarousel.CurrentItem is Movie portraitMovie)
        {
            currentMovie = portraitMovie;
        }

        if (currentMovie == null) return;

        bool alreadySaved = await Models.SavedFilms.IsMovieSavedAsync(currentMovie.TMDBId);
        if (alreadySaved)
        {
            DisplayAlert("Already Saved", $"{currentMovie.Title} is already saved.", "OK");
        }
        else
        {
            await Models.SavedFilms.AddMovieAsync(currentMovie);
            DisplayAlert("Saved", $"{currentMovie.Title} has been added to your bookmarks.", "OK");
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

    private void LandscapeCarousel_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (LandscapeCarousel.CurrentItem is Movie currentMovie)
        {
            LandscapeSynopsis.Text = currentMovie.Overview;
        }
    }
}