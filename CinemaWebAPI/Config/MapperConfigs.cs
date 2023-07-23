using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;

namespace CinemaWebAPI.Config
{
    public class MapperConfigs : Profile
    {
        public MapperConfigs()
        {
            CreateMap<Movie, MovieDTO>()
           .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Description))
           .ReverseMap();

            CreateMap<MovieDTO, Movie>()
               .ForMember(dest => dest.Genre, opt => opt.Ignore()) // Ignore Genre mapping
               .ForMember(dest => dest.Rates, opt => opt.Ignore()); // Ignore Rates mapping

            CreateMap<Genre, GenreDTO>().ReverseMap();

        }
    }
}
