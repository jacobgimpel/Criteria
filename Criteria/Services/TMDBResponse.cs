using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Criteria.Models;

namespace Criteria.Services
{
    class TMDBResponse
    {
        public List<Movie>? results { get; set; }
    }

    public class NewMovie
    {
        [JsonProperty("id")]
        public string? TMDBid { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("poster_path")]
        public string? PosterPath { get; set; }
    }
}
