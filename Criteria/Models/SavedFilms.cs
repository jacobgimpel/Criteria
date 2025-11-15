using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Criteria.Models
{
    public class SavedFilms
    {
        public static List<Movie> SavedMovies { get; set; } = new List<Movie>();

        public static void AddMovie(Movie movie)
        {
            if (!SavedMovies.Any(m => m.TMDBId == movie.TMDBId))
            {
                SavedMovies.Add(movie);
            }
        }

        public static void RemoveMovie(Movie movie)
        {
            if (SavedMovies.Contains (movie))
                SavedMovies.Remove(movie);
        }
    }
}
