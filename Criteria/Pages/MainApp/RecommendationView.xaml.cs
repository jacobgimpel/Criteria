using Criteria.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
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

    private async void OnPosterTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image image && image.Parent is Grid grid)
        {
            var overlay = grid.FindByName<Grid>("Overlay");
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
