namespace Criteria.Pages.FirstBoot;

public partial class FilmSelectionPage : ContentPage
{
	private readonly List<string> _selectedGenres;
    public FilmSelectionPage(List<string> selectedGenres)
    {
        InitializeComponent();
        _selectedGenres = selectedGenres;
    }
}