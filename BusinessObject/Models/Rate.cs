using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class Rate
    {
        [Key]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        [Key]
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public string Comment { get; set; }
        public decimal? NumericRating { get; set; }
        public DateTime? Time { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
