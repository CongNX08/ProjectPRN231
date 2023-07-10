using AutoMapper;
using BusinessObject;
using CinemaWebAPI.Config;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Add sql server
            builder.Services.AddDbContext<CinemaDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            });

            //Add Interface
            builder.Services.AddScoped<MovieRepository>();
            builder.Services.AddScoped<PersonRepository>();
            builder.Services.AddScoped<RateRepository>();
            builder.Services.AddScoped<GenreRepository>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //add automapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigs());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}