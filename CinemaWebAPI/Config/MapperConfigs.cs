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
           .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
           .ReverseMap();

        }
    }
}
