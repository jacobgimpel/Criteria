using Criteria.Models;
using Microsoft.Maui.Controls;

namespace Criteria.Pages.MainApp;

public partial class SavedFilms : ContentPage
{
    private Movie _selectedMovie;

    public SavedFilms()
    {
        InitializeComponent();

        PortraitLayout.ItemsSource = Models.SavedFilms.SavedMovies;
        LandscapeCarousel.ItemsSource = Models.SavedFilms.SavedMovies;

        SizeChanged += SavedFilms_SizeChanged;
    }

    private void SavedFilms_SizeChanged(object sender, EventArgs e)
    {
        if (Width > Height) //Landscape
        {
            TitleLabel.IsVisible = false;
            ControlsLayout.IsVisible = false;
            PortraitLayout.IsVisible = false;
            LandscapeLayout.IsVisible = true;
            UpdateLandscapeContent();
        }
        else //Portrait
        {
            TitleLabel.IsVisible = true;
            ControlsLayout.IsVisible = true;
            PortraitLayout.IsVisible = true;
            LandscapeLayout.IsVisible = false;
        }
    }

    private void PortraitLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedMovie = e.CurrentSelection.FirstOrDefault() as Movie;
        UpdateLandscapeContent();
    }

    private void LandscapeCarousel_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        _selectedMovie = LandscapeCarousel.CurrentItem as Movie;
        UpdateLandscapeContent();
    }

    private void UpdateLandscapeContent()
    {
        if (_selectedMovie != null)
        {
            LandscapeSynopsis.Text = _selectedMovie.Overview;
        }
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (_selectedMovie == null)
        {
            await DisplayAlert("No movie selected", "Please select a movie to delete.", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirm Deletion",
            $"Are you sure you want to delete {_selectedMovie.Title} from your saved films?", "Yes", "No");

        if (confirm)
        {
            Models.SavedFilms.SavedMovies.Remove(_selectedMovie);
            PortraitLayout.ItemsSource = null;
            PortraitLayout.ItemsSource = Models.SavedFilms.SavedMovies;

            LandscapeCarousel.ItemsSource = null;
            LandscapeCarousel.ItemsSource = Models.SavedFilms.SavedMovies;

            _selectedMovie = null;
        }
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnPosterTapped(object sender, EventArgs e)
    {
        if (Width <= Height)

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
}
