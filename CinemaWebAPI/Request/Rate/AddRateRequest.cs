namespace CinemaWebAPI.Request.Rate
{
    public class AddRateRequest
    {
        public int MovieId { get; set; }
        public int PersonId { get; set; }

        public string Comment { get; set; }
        public decimal? NumericRating { get; set; }
    }
}