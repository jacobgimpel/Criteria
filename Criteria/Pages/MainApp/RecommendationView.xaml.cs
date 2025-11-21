using Criteria.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace Criteria.Pages.MainApp;

public partial class RecommendationView : ContentPage
{
    private readonly List<Movie> _recommendedMovies;

    public RecommendationView(List<Movie> recommendedMovies)
    {
        InitializeComponent();
        _recommendedMovies = recommendedMovies ?? new List<Movie>();

        RecommendationsCarousel.ItemsSource = _recommendedMovies;
        LandscapeCarousel.ItemsSource = _recommendedMovies;

        RecommendationsCarousel.PositionChanged += (s, e) =>
        {
            if (RecommendationsCarousel.CurrentItem is Movie movie)
            {
                LandscapeSynopsis.Text = movie.Overview;
            }
        };
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
        Movie currentMovie = null;

        if (LandscapeGrid.IsVisible && LandscapeCarousel.CurrentItem is Movie landscapeMovie)
        {
            currentMovie = landscapeMovie;
        }
        else if (RecommendationsCarousel.CurrentItem is Movie portraitMovie)
        {
            currentMovie = portraitMovie;
        }

        if (currentMovie == null) return;

        bool alreadySaved = Models.SavedFilms.SavedMovies.Exists(m => m.TMDBId == currentMovie.TMDBId);
        if (alreadySaved)
        {
            DisplayAlert("Already Saved", $"{currentMovie.Title} is already saved.", "OK");
        }
        else
        {
            Models.SavedFilms.AddMovie(currentMovie);
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

    private void RecommendationView_SizeChanged(object sender, EventArgs e)
    {
        if (Width > Height)
        {
            AppTitle.IsVisible = false;
            InstructionsStack.IsVisible = false;
            FooterGrid.IsVisible = false;
            RecommendationsCarousel.IsVisible = false;

            LandscapeGrid.IsVisible = true;

            if (LandscapeCarousel.CurrentItem is Movie currentMovie)
                LandscapeSynopsis.Text = currentMovie.Overview;
        }
        else
        {
            AppTitle.IsVisible = true;
            InstructionsStack.IsVisible = true;
            FooterGrid.IsVisible = true;
            RecommendationsCarousel.IsVisible = true;

            LandscapeGrid.IsVisible = false;
        }
    }

    private void LandscapeCarousel_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (LandscapeCarousel.CurrentItem is Movie currentMovie)
        {
            LandscapeSynopsis.Text = currentMovie.Overview;
        }
    }
}
