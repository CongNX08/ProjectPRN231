using System.ComponentModel.DataAnnotations;

namespace CinemaWebAPI.Request.Rate
{
    public class AddRateRequest
    {
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Required]
        public string Comment { get; set; }
        public decimal? NumericRating { get; set; } = (decimal?)0.0;
    }
}
