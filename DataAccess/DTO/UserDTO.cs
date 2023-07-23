using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class UserDTO
    {
        public int PersonId { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }

        public string Email { get; set; }
        public int? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
