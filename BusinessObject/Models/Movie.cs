using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Rates = new HashSet<Rate>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        public decimal? RatingPoint { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }

    }
}
