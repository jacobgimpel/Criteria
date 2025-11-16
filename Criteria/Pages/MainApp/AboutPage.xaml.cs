using Microsoft.Maui.Controls;

namespace Criteria.Pages.MainApp
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            VersionLabel.Text = "Version number: " + AppInfo.VersionString;
        }

        private void OnHomeClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
