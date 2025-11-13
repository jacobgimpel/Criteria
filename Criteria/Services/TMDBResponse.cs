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
        [JsonProperty("results")]
        public List<NewMovie>? Results { get; set; }
    }

    public class NewMovie
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("poster_path")]
        public string? PosterPath { get; set; }

        [JsonProperty("overview")]
        public string? Overview { get; set; }

    }
}