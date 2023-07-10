using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;

namespace CinemaWebAPI.Config
{
    public class MapperConfigs : Profile
    {
        public MapperConfigs()
        {
            ////Movie
            //CreateMap<Movie, MovieDTO>()
            //.ForMember(dest => dest.Publisher_name, opt => opt.MapFrom(src => src.Publisher.Publisher_name));
            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieDTO, Movie>().ReverseMap();
            ////Person
            //CreateMap<Person, PersonDTO>().ReverseMap();
            //CreateMap<AuthorDTO, Author>().ReverseMap();
            ////Publisher
            //CreateMap<Publisher, PublisherDTO>().ReverseMap();
            //CreateMap<PublisherDTO, Publisher>().ReverseMap();
            ////User
            //CreateMap<User, UserDTO>().ReverseMap();
            //CreateMap<UserDTO, User>().ReverseMap();

        }
    }
}
