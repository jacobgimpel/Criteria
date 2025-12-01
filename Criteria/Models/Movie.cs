using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Criteria.Models
{
    public class Movie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? PosterPath { get; set; }
        public string? TMDBId { get; set; }

        public string? Overview { get; set; }
    }
}
