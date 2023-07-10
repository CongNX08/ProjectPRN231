using BusinessObject;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MovieRepository : Repository<Movie>
    {
        private readonly CinemaDbContext _db;
        public MovieRepository(CinemaDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
