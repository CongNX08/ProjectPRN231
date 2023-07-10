using BusinessObject;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PersonRepository : Repository<Person>
    {
        private readonly CinemaDbContext _db;
        public PersonRepository(CinemaDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
