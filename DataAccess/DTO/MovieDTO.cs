using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class MovieDTO
    {
        public int CountNumberofResult { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
        public string? Genre { get; set; }
        public decimal? RatingPoint { get; set; }
    }
}
