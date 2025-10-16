using Criteria.Models;
using Criteria.Services;

namespace Criteria.Pages.FirstBoot;

public partial class FilmSelectionPage : ContentPage
{
	private readonly List<string> _selectedGenres;
    private readonly List<Movie> _selectedMovies = new();
    private readonly TMDBService _tmdbService = new();
    public FilmSelectionPage(List<string> selectedGenres)
    {
        InitializeComponent();
        _selectedGenres = selectedGenres;

        ContinueButton.IsEnabled = false;
        ContinueButton.BackgroundColor = Colors.Gray;
        CounterLabel.Text = $"0/10";
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var query = SearchBar.Text?.Trim();
        if (string.IsNullOrEmpty(query))
            return;
        var movie = await _tmdbService.SearchMovieAsync(query);
        if (movie == null)
        {
            await DisplayAlert("Not Found", "Movie not found. Please try another title.", "OK");
            return;
        }
        if (_selectedMovies.Any(m => m.TMDBId == movie.TMDBId))
        {
            await DisplayAlert("Already Added", "This movie is already in your selection.", "OK");
            return;
        }
        _selectedMovies.Add(movie);
        AddMoviePoster(movie);
        UpdateCounter();
        UpdateContinueButton();
    }

    private void AddMoviePoster(Movie movie)
    {
        var image = new Image
        {
            Source = $"https://image.tmdb.org/t/p/w200{movie.PosterPath}",
            WidthRequest = 70,
            HeightRequest = 100,
        };

        SelectedMoviesLayout.Children.Add(image);
    }
    private void UpdateCounter()
    {
        CounterLabel.Text = $"{_selectedMovies.Count}/10";
    }
    private void UpdateContinueButton()
    {
        if (_selectedMovies.Count == 10)
        {
            ContinueButton.IsEnabled = true;
            ContinueButton.BackgroundColor = Colors.Blue;
        }
        else
        {
            ContinueButton.IsEnabled = false;
            ContinueButton.BackgroundColor = Colors.Gray;
        }
    }
    private async void OnContinueButtonClicked(object sender, EventArgs e)
    {
        if (_selectedMovies.Count != 10)
            return;
        await Navigation.PushAsync(new LoadingPage());
    }

}