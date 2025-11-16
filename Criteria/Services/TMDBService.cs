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
        private string _apiKey = "2f4ba5fc63c680d7a0d0b11ffc80b780";
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        private static readonly HttpClient client = new HttpClient();
        private readonly Random _random = new Random();

        private readonly Dictionary<string, string> GenreMapping = new()
        {
            {"Action", "28"}, {"Comedy", "35"}, {"Drama", "18"}, {"Horror", "27"},
            {"Romance", "10749"}, {"Thriller", "53"}, {"Sci-fi", "878"}, {"Fantasy", "14"},
            {"Documentary", "99"}, {"Animation", "16"}, {"Mystery", "9648"}, {"Adventure", "12"}
        };


        public async Task<List<Movie>> SearchMovieAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Movie>();

            var url = $"{BaseUrl}/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}";

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
                    var response = await client.GetStringAsync($"{BaseUrl}/movie/{movie.TMDBId}/similar?api_key={_apiKey}");
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

                int randomPage = _random.Next(1, 6);

                try
                {
                    var response = await client.GetStringAsync($"{BaseUrl}/discover/movie?api_key={_apiKey}&with_genres={genreId}&sort_by=popularity.desc&page=1");
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
                .OrderBy(_=> _random.Next())
                .Take(maxResults)
                .ToList();
        }
    }
}
