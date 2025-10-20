using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Criteria.Models;

namespace Criteria.Services
{
    public class TMDBService
    {
        private const string ApiKey = "bfd8a54def262291ca861cc2a50e52ad";
        private const string BaseUrl = "https://api.themoviedb.org/3";

        public async Task<Movie?> SearchMovieAsync(string query)
        {
            using var client = new HttpClient();
            var url = $"{BaseUrl}/search/movie?api_key={ApiKey}&query={Uri.EscapeDataString(query)}";
            var response = await client.GetStringAsync(url);

            var searchResult = JsonConvert.DeserializeObject<TMDBResponse>(response);
            var firstMovie = searchResult?.results?.FirstOrDefault();

            return firstMovie;
        }
    }
}
