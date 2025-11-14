using System;
using Microsoft.Maui.Controls;

namespace Criteria.Pages.MainApp
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            // Navigate back to MainPage
            await Navigation.PushAsync(new MainPage());
        }
    }
}
