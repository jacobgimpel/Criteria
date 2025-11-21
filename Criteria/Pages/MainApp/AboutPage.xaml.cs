using Microsoft.Maui.Controls;

namespace Criteria.Pages.MainApp
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            VersionLabel.Text = "Version number: " + AppInfo.VersionString;
            this.Appearing += (s, e) => Services.OrientationService.LockPortrait();
            this.Disappearing += (s, e) => Services.OrientationService.UnlockOrientation();
        }

        private void OnHomeClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
