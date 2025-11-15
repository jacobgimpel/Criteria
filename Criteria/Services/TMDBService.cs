using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Criteria.Models;
using Newtonsoft.Json;

namespace Criteria.Services
{
    public class TMDBService
    {
        private const string ApiKey = "bfd8a54def262291ca861cc2a50e52ad";
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        private static readonly HttpClient client = new HttpClient();

        private readonly Dictionary<string, string> GenreMapping = new()
        {
            {"Action", "28"}, {"Comedy", "35"}, {"Drama", "18"}, {"Horror", "27"},
            {"Romance", "10749"}, {"Thriller", "53"}, {"Sci-fi", "878"}, {"Fantasy", "14"},
            {"Documentary", "99"}, {"Animation", "16"}, {"Mystery", "9648"}, {"Adventure", "12"}
        };

        /// <summary>
        /// Search for movies by query.
        /// </summary>
        public async Task<List<Movie>> SearchMovieAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Movie>();

            var url = $"{BaseUrl}/search/movie?api_key={ApiKey}&query={Uri.EscapeDataString(query)}";

            try
            {
                var response = await client.GetStringAsync(url);
                var tmdbResponse = JsonConvert.DeserializeObject<TMDBResponse>(response);

                if (tmdbResponse?.Results == null)
                    return new List<Movie>();

                return tmdbResponse.Results
                    .Where(r => !string.IsNullOrEmpty(r.PosterPath))
                    .Select(r => new Movie
                    {
                        Title = r.Title,
                        PosterPath = ImageBaseUrl + r.PosterPath,
                        TMDBId = r.Id.ToString(),
                        Overview = r.Overview
                    })
                    .ToList();
            }
            catch
            {
                return new List<Movie>();
            }
        }

        public async Task<List<Movie>> GetRecommendationsAsync(List<Movie> selectedMovies, List<string> selectedGenres, int maxResults = 5)
        {
            var recommendedMovies = new List<Movie>();
            var existingMovieIds = selectedMovies.Select(m => m.TMDBId).ToHashSet();


            foreach (var movie in selectedMovies)
            {
                if (string.IsNullOrEmpty(movie.TMDBId)) continue;

                try
                {
                    var response = await client.GetStringAsync($"{BaseUrl}/movie/{movie.TMDBId}/similar?api_key={ApiKey}");
                    var tmdbResponse = JsonConvert.DeserializeObject<TMDBResponse>(response);

                    if (tmdbResponse?.Results != null)
                    {
                        recommendedMovies.AddRange(tmdbResponse.Results
                            .Where(r => !existingMovieIds.Contains(r.Id.ToString()) && !string.IsNullOrEmpty(r.PosterPath))
                            .Select(r => new Movie
                            {
                                Title = r.Title,
                                PosterPath = ImageBaseUrl + r.PosterPath,
                                TMDBId = r.Id.ToString(),
                                Overview = r.Overview
                            }));
                    }
                }
                catch { continue; }
            }

            foreach (var genreTitle in selectedGenres)
            {
                if (!GenreMapping.TryGetValue(genreTitle, out var genreId)) continue;

                try
                {
                    var response = await client.GetStringAsync($"{BaseUrl}/discover/movie?api_key={ApiKey}&with_genres={genreId}&sort_by=popularity.desc&page=1");
                    var tmdbResponse = JsonConvert.DeserializeObject<TMDBResponse>(response);

                    if (tmdbResponse?.Results != null)
                    {
                        recommendedMovies.AddRange(tmdbResponse.Results
                            .Where(r => !existingMovieIds.Contains(r.Id.ToString()) && !string.IsNullOrEmpty(r.PosterPath))
                            .Select(r => new Movie
                            {
                                Title = r.Title,
                                PosterPath = ImageBaseUrl + r.PosterPath,
                                TMDBId = r.Id.ToString(),
                                Overview = r.Overview
                            }));
                    }
                }
                catch { continue; }
            }

            return recommendedMovies
                .GroupBy(m => m.TMDBId)
                .Select(g => g.First())
                .Take(maxResults)
                .ToList();
        }
    }
}
