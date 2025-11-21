using Criteria.Models;
using System.Threading.Tasks;

namespace Criteria.Pages.MainApp;

public partial class SavedFilms : ContentPage
{
    private Movie _selectedMovie;
    public SavedFilms()
    {
        InitializeComponent();

        SavedFilmsCollection.SelectionChanged += OnMovieSelected;
        SavedFilmsCollection.ItemsSource = Models.SavedFilms.SavedMovies;

    }

    private void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        _selectedMovie = e.CurrentSelection.FirstOrDefault() as Movie;
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (_selectedMovie == null)
        {
            await DisplayAlert("No movie selected", "Please select a movie to delete.", "OK");
            return;
        }
        bool confirm = await DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {_selectedMovie.Title} from your saved films?", "Yes", "No");
        if (confirm)
        {
            Models.SavedFilms.SavedMovies.Remove(_selectedMovie);
            SavedFilmsCollection.ItemsSource = null;
            SavedFilmsCollection.ItemsSource = Models.SavedFilms.SavedMovies;
            _selectedMovie = null;
        }

    }

    private async void OnPosterTapped(object sender, EventArgs e)
    {
        if (sender is Image image)
        {
            var parentGrid = image.Parent as Grid;
            if (parentGrid == null)
                return;

            var overlay = parentGrid.FindByName<Grid>("Overlay");
            if (overlay == null)
                return;

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

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

}