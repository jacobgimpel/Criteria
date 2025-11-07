using Criteria.Models;
using Criteria.Services;

namespace Criteria.Pages.FirstBoot;

public partial class FilmSelectionPage : ContentPage
{
    private readonly TMDBService _tmdbService = new();
    private readonly List<Movie> searchResults = new();
    private Movie? _highlightedMovie;
    private readonly List<Movie> _selectedMovies = new();
    private readonly List<string> _selectedGenres = new();

    public FilmSelectionPage()
    {
        InitializeComponent();
        SearchBar.TextChanged += OnSearchTextChanged;
    }

    public FilmSelectionPage(List<string> selectedGenres) : this()
    {
        _selectedGenres = selectedGenres;
    }

    private async void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        string query = e.NewTextValue?.Trim() ?? "";
        if (string.IsNullOrEmpty(query))
            return;

        await SearchAndDisplayMoviesAsync(query);
    }

    private async Task SearchAndDisplayMoviesAsync(string query)
    {
        searchResults.Clear();
        SearchResultsGrid.Children.Clear();
        SearchResultsGrid.RowDefinitions.Clear();
        SearchResultsGrid.ColumnDefinitions.Clear();


        for (int i = 0; i < 3; i++)
            SearchResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        var movieResults = await _tmdbService.SearchMovieAsync(query);

        if (movieResults != null)
        {
    
            searchResults.AddRange(movieResults.Take(5));
        }

        int row = 0, col = 0;
        foreach (var result in searchResults)
        {

            if (string.IsNullOrEmpty(result.PosterPath))
                continue;

            var posterImage = new Image
            {
                Source = result.PosterPath,
                Aspect = Aspect.AspectFill,
                HeightRequest = 200,
                WidthRequest = 150
            };

            var tapGesture = new TapGestureRecognizer();
    
            tapGesture.Tapped += (s, e) => OnMovieSelected(result, posterImage);
            posterImage.GestureRecognizers.Add(tapGesture);

   
            if (SearchResultsGrid.RowDefinitions.Count <= row)
                SearchResultsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid.SetColumn(posterImage, col);
            Grid.SetRow(posterImage, row);
            SearchResultsGrid.Children.Add(posterImage);

            col++;
            if (col >= 3)
            {
                col = 0;
                row++;
            }
        }
    }

    private void OnMovieSelected(Movie movie, Image posterImage)
    {
        _highlightedMovie = movie;

        foreach (var child in SearchResultsGrid.Children)
            if (child is Image img)
                img.Opacity = 1.0;

        posterImage.Opacity = 0.5;
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (_highlightedMovie == null)
        {
            await DisplayAlert("No Movie Selected", "Please select a movie to add.", "OK");
            return;
        }

        if (_selectedMovies.Count >= 10)
        {
            await DisplayAlert("Limit Reached", "You can only add 10 movies.", "OK");
            return;
        }

        if (_selectedMovies.Any(m => m.TMDBId == _highlightedMovie.TMDBId))
        {
            await DisplayAlert("Already Added", "This movie has already been added.", "OK");
            return;
        }

        _selectedMovies.Add(_highlightedMovie);

        var selectedPoster = new Image
        {
            Source = _highlightedMovie.PosterPath,
            Aspect = Aspect.AspectFill,
            HeightRequest = 100,
            WidthRequest = 75,
            Margin = new Thickness(5)
        };

        SelectedMoviesLayout.Children.Add(selectedPoster);

        CounterLabel.Text = $"{_selectedMovies.Count}/10 Movies Selected";
        ContinueButton.IsEnabled = _selectedMovies.Count == 10;
        ContinueButton.BackgroundColor = ContinueButton.IsEnabled ? Colors.Purple : Colors.Gray;

        foreach (var child in SearchResultsGrid.Children)
            if (child is Image img)
                img.Opacity = 1.0;

        _highlightedMovie = null;
    }

    private async void OnContinueButtonClicked(object sender, EventArgs e)
    {
        if (_selectedMovies.Count < 10)
        {
            await DisplayAlert("Too Few Movies", "Please select 10 movies to continue.", "OK");
            return;
        }
        if (_selectedMovies.Count == 10)
        {
            await Navigation.PushAsync(new Pages.FirstBoot.LoadingPage(_selectedGenres, _selectedMovies));
        }

    }
}
