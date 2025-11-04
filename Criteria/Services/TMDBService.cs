using Newtonsoft.Json;
using Criteria.Models;

namespace Criteria.Services
{
    public class TMDBService
    {
        private const string ApiKey = "bfd8a54def262291ca861cc2a50e52ad";
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

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
    }
}