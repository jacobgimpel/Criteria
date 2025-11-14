using Criteria.Models;
using Criteria.Pages.MainApp;

namespace Criteria
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Pages.MainApp.MainPage());
        }
    }
}
