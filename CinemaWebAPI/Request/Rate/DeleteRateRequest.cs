using System.ComponentModel.DataAnnotations;

namespace CinemaWebAPI.Request.Rate
{
    public class DeleteRateRequest
    {
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int PersonId { get; set; }
    }
}
