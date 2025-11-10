using Newtonsoft.Json;
using Criteria.Models;
using System.Net.Http;

namespace Criteria.Services
{
    public class TMDBService
    {
        private const string ApiKey = "bfd8a54def262291ca861cc2a50e52ad";
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        private readonly Dictionary<string, string> GenreMapping = new()
        {
            {"Action", "28"},
            {"Comedy", "35"},
            {"Drama", "18"},
            {"Horror", "27"},
            {"Romance", "10749"},
            {"Thriller", "53"},
            {"Sci-fi", "878"},
            {"Fantasy", "14"},
            {"Documentary", "99"},
            {"Animation", "16"},
            {"Mystery", "9648"},
            {"Adventure", "12"}
        };

        public async Task<List<Movie>> SearchMovieAsync(string query)
        {
            using var client = new HttpClient();
            var url = $"{BaseUrl}/search/movie?api_key={ApiKey}&query={Uri.EscapeDataString(query)}";
            var response = await client.GetStringAsync(url);
            var searchResult = JsonConvert.DeserializeObject<TMDBResponse>(response);

            if (searchResult?.Results == null)
                return new List<Movie>();

            var movies = searchResult.Results
                .Select(tmdbResult => new Movie
                {
                    Title = tmdbResult.Title,
                    PosterPath = tmdbResult.PosterPath != null
                        ? ImageBaseUrl + tmdbResult.PosterPath
                        : null,
                    TMDBId = tmdbResult.Id.ToString()
                })
                .Where(m => !string.IsNullOrEmpty(m.PosterPath))
                .ToList();

            return movies;
        }

        public async Task<List<Movie>> GetRecommendationsAsync(List<Movie> selectedMovies, List<string> selectedGenres, int maxResults = 5)
        {
            var recommendedMovies = new List<Movie>();
            var existingMovieIds = selectedMovies.Select(m => m.TMDBId).ToList();
            using var client = new HttpClient();

            foreach (var movie in selectedMovies)
            {
                if (string.IsNullOrEmpty(movie.TMDBId))
                    continue;

                var url = $"{BaseUrl}/movie/{movie.TMDBId}/similar?api_key={ApiKey}";
                try
                {
                    var response = await client.GetStringAsync(url);
                    var tmdbResponse = JsonConvert.DeserializeObject<TMDBResponse>(response);

                    if (tmdbResponse?.Results == null)
                        continue;

                    foreach (var result in tmdbResponse.Results)
                    {
                        if (!existingMovieIds.Contains(result.Id.ToString()) && !string.IsNullOrEmpty(result.PosterPath))
                        {
                            recommendedMovies.Add(new Movie
                            {
                                Title = result.Title,
                                PosterPath = ImageBaseUrl + result.PosterPath,
                                TMDBId = result.Id.ToString()
                            });
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            foreach (var genreTitle in selectedGenres)
            {
                if (!GenreMapping.TryGetValue(genreTitle, out var genreId))
                    continue;

                var url = $"{BaseUrl}/discover/movie?api_key={ApiKey}&with_genres={genreId}&sort_by=popularity.desc&page=1";
                try
                {
                    var response = await client.GetStringAsync(url);
                    var tmdbResponse = JsonConvert.DeserializeObject<TMDBResponse>(response);

                    if (tmdbResponse?.Results == null)
                        continue;

                    foreach (var result in tmdbResponse.Results)
                    {
                        if (!existingMovieIds.Contains(result.Id.ToString()) && !string.IsNullOrEmpty(result.PosterPath))
                        {
                            recommendedMovies.Add(new Movie
                            {
                                Title = result.Title,
                                PosterPath = ImageBaseUrl + result.PosterPath,
                                TMDBId = result.Id.ToString()
                            });
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            recommendedMovies = recommendedMovies
                .GroupBy(m => m.TMDBId)
                .Select(g => g.First())
                .ToList();

            return recommendedMovies.Take(maxResults).ToList();
        }
    }
}
