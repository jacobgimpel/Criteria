using Criteria.Models; 

namespace Criteria.Pages.FirstBoot;

public partial class LoadingPage : ContentPage
{
    private readonly List<Movie> _selectedMovies;
    private readonly List<string> _selectedGenres;

    public LoadingPage(List<string> selectedGenres, List<Movie> selectedMovies)
    {
        InitializeComponent();
        _selectedGenres = selectedGenres;
        _selectedMovies = selectedMovies;

    }

    public LoadingPage()
    {
        InitializeComponent();
    }
}
