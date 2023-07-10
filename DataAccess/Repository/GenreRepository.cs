using BusinessObject;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class GenreRepository : Repository<Genre>
    {


        private readonly CinemaDbContext _db;
        public GenreRepository(CinemaDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
