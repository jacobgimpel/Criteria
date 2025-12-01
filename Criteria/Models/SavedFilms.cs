using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Criteria.Models
{
    public static class SavedFilms
    {
        public static async Task<List<Movie>> LoadSavedMoviesAsync()
        {
            return await App.DatabaseService.GetMoviesAsync();
        }

        public static async Task AddMovieAsync(Movie movie)
        {
            await App.DatabaseService.SaveMovieAsync(movie);
        }

        public static async Task DeleteMovieAsync(Movie movie)
        {
            await App.DatabaseService.DeleteMovieAsync(movie);
        }

        public static async Task<bool> IsMovieSavedAsync(string tmdbId)
        {
            var savedMovies = await LoadSavedMoviesAsync();
            return savedMovies.Any(m => m.TMDBId == tmdbId);
        }

    }
    
}
