using Criteria.Models;
using Criteria.Services;
using Criteria.Utilities;
using Criteria.Pages.MainApp;
using System.IO;

namespace Criteria
{
    public partial class App : Application
    {
        private static SQLService databaseService = default;
        public static SQLService DatabaseService
        {
            get
            {
                if(databaseService == null)
                {
                    databaseService = new SQLService(
                        Path.Combine(
                            Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData), "criteria.db"));
                }
                return databaseService;
            }
        }
        public App()
        {
            InitializeComponent();

            bool firstBootDone = Preferences.Get(SetPreferences.SelectionsCompleted, false);
            if (firstBootDone)
            {
                MainPage = new NavigationPage(new Pages.MainApp.MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new Pages.FirstBoot.GenreSelectionPage());
            }
        }
    }
}
