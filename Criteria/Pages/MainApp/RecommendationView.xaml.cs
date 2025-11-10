using Criteria.Models;
using Newtonsoft.Json;
namespace Criteria.Pages.MainApp;

public partial class RecommendationView : ContentPage
{
    private readonly List<Movie> _recommendedMovies;

    public RecommendationView(List<Movie> recommendedMovies)
    {
        InitializeComponent();
        _recommendedMovies = recommendedMovies;

        RecommendationsCarousel.ItemsSource = _recommendedMovies;
    }

    private void OnPosterTapped(object sender, EventArgs e)
    {
        DisplayAlert("Great work for tapping a movie, awesome!", "Ok", "This is just a test");
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        DisplayAlert("You saved", "Awesome", "Nice");
    }

    private void OnHomeClicked(object sender, EventArgs e)
    {
        DisplayAlert("Home clicked", "Navigating to home", "Ok");
    }

    private void OnBookmarkClicked(object sender, EventArgs e)
    {
        DisplayAlert("Bookmarks clicked", "Navigating to bookmarks", "Ok");
    }
}
