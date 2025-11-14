using System.Threading.Tasks;

namespace Criteria.Pages.MainApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnFindButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecommendationView(new List<Models.Movie>()));
    }

    private async void OnSavedFilmsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SavedFilms());
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}
