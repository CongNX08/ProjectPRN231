using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class Person
    {
        public Person()
        {
            Rates = new HashSet<Rate>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
       
        public string Email { get; set; }
        
        public string Password { get; set; }
        public int? Type { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}
