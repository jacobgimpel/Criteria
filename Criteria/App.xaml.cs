using Criteria.Models;
using Criteria.Pages.MainApp;

namespace Criteria
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Placeholder movies to test 
            var placeholderMovies = new List<Movie>
            {
                new Movie { Title = "Inception", PosterPath = "/inception.jpg"},
                new Movie { Title = "Interstellar", PosterPath = "/interstellar.jpg"},
                new Movie { Title = "The Dark Knight", PosterPath = "/darkknight.jpg"}
            };

            //Pass test movies to page
            MainPage = new NavigationPage(new RecommendationView(placeholderMovies));
        }
    }
}
