namespace Criteria.Pages.MainApp;

public partial class SavedFilms : ContentPage
{
	public SavedFilms()
	{
		InitializeComponent();

		SavedFilmsCollection.ItemsSource = Models.SavedFilms.SavedMovies;
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
}