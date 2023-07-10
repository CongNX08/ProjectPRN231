using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace BusinessObject
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext()
        {
            
        }
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasKey(ba => new { ba.MovieId, ba.PersonId });
            modelBuilder.Entity<Person>()
               .HasKey(ba => new { ba.PersonId });
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }


    }
}
