using Newtonsoft.Json;
using Criteria.Models;

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
            {"Drama", "18" },
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
    }
}