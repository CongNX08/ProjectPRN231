using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class RateDTO
    {
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string Comment { get; set; }
        public decimal? NumericRating { get; set; }
        public string? Time { get; set; }
    }
}
