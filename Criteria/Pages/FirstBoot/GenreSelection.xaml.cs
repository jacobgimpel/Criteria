
using Criteria.Services;
using System.Threading.Tasks;

namespace Criteria.Pages.FirstBoot;

public partial class GenreSelectionPage : ContentPage
{
	public List<string> SelectedGenres { get; } = new List<string>();
    public GenreSelectionPage()
    {
        InitializeComponent();

		this.Appearing += (s, e) => Services.OrientationService.LockPortrait();
		this.Disappearing += (s, e) => Services.OrientationService.UnlockOrientation();
    }


    private void OnGenreButtonClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			string genre = button.Text;
			if (SelectedGenres.Contains(genre))
			{
				SelectedGenres.Remove(genre);
				button.BackgroundColor = Color.FromArgb("#E0E0E0");
			}
			else
			{
				SelectedGenres.Add(genre);
				button.BackgroundColor = Colors.Purple;
			}

			ContinueButton.IsEnabled = SelectedGenres.Count > 0;
			ContinueButton.BackgroundColor = ContinueButton.IsEnabled ? Colors.Purple : Colors.Gray;
        }
	}
	private async void OnContinueButtonClicked(object sender, EventArgs e)
    {
		if (SelectedGenres.Count == 0)
			return;

		await Navigation.PushAsync(new Pages.FirstBoot.FilmSelectionPage(SelectedGenres));

    }

}