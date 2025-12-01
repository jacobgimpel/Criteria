using Criteria.Models;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Criteria.Pages.MainApp;

public partial class SavedFilms : ContentPage
{
    private Movie _selectedMovie;
    private Grid _currentOpenOverlay = null;

    public SavedFilms()
    {
        InitializeComponent();

        SizeChanged += SavedFilms_SizeChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadSavedMoviesAsync();
    }

    private async Task LoadSavedMoviesAsync()
    {
        var savedMovies = await Models.SavedFilms.LoadSavedMoviesAsync();

        if (savedMovies == null)
        {
            savedMovies = new List<Movie>();
        }

        PortraitLayout.ItemsSource = savedMovies;
        LandscapeCarousel.ItemsSource = savedMovies;
    }

    private void SavedFilms_SizeChanged(object sender, EventArgs e)
    {
        if (Width > Height)
        {
            TitleLabel.IsVisible = false;
            ControlsLayout.IsVisible = false;
            PortraitLayout.IsVisible = false;
            LandscapeLayout.IsVisible = true;
            UpdateLandscapeContent();
        }
        else
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
            await Models.SavedFilms.DeleteMovieAsync(_selectedMovie);
            await LoadSavedMoviesAsync();
            _selectedMovie = null;
            UpdateLandscapeContent();
        }
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnPosterTapped(object sender, EventArgs e)
    {
        if (Width > Height)
            return;

        if (sender is not Image tappedImage)
            return;
        
        if (tappedImage.BindingContext is Movie tappedMovie)
        {
            _selectedMovie = tappedMovie;
            PortraitLayout.SelectedItem = tappedMovie;
        }

        if (tappedImage.Parent is not Grid parentGrid)
            return;

        var thisOverlay = parentGrid.FindByName<Grid>("Overlay");
        if (thisOverlay == null)
            return;

        if (_currentOpenOverlay != null && _currentOpenOverlay != thisOverlay)
        {
            _currentOpenOverlay.IsVisible = false;
            _currentOpenOverlay.Opacity = 0;
        }

        if (thisOverlay.IsVisible)
        {
            await thisOverlay.FadeTo(0, 150);
            thisOverlay.IsVisible = false;
            _currentOpenOverlay = null;
        }
        else
        {
            thisOverlay.IsVisible = true;
            await thisOverlay.FadeTo(1, 150);
            _currentOpenOverlay = thisOverlay;
        }
    }

    private async void OnOverlayTapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid overlay)
        {
            await overlay.FadeTo(0, 250);
            overlay.IsVisible = false;

            if (_currentOpenOverlay == overlay)
                _currentOpenOverlay = null;
        }
    }
}