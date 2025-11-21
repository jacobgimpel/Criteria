using System;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using Criteria.Utilities;

namespace Criteria.Pages.MainApp
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.Appearing += (s, e) => Services.OrientationService.LockPortrait();
            this.Disappearing += (s, e) => Services.OrientationService.UnlockOrientation();
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Pages.MainApp.AboutPage());
        }

        private async void OnResetPreferencesClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Reset Preferences", 
                "Are you sure you want to reset your preferences? This will take you back to the initial setup.", 
                "Yes", 
                "No");
            if (!confirm) return; 
            
            Preferences.Remove(SetPreferences.SelectedGenres);
            Preferences.Remove(SetPreferences.SelectedMovies);
            Preferences.Remove(SetPreferences.SelectionsCompleted);

            Application.Current.MainPage = new NavigationPage(new Pages.FirstBoot.GenreSelectionPage());
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Pages.MainApp.MainPage());
        }
    }
}
