using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class Rate
    {
        [Key]
        public int MovieId { get; set; }
        [Key]
        public int PersonId { get; set; }
        public string Comment { get; set; }
        public decimal? NumericRating { get; set; }
        public DateTime? Time { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
