using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly GenreRepository _dbGenre;
        private readonly IMapper _mapper;
        public GenreController(GenreRepository dbGenre, IMapper mapper)
        {
            _dbGenre = dbGenre;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {

            IEnumerable<Genre> GenreList = await _dbGenre.GetAllAsync();
            IEnumerable<GenreDTO> GenreDTOList = _mapper.Map<IEnumerable<GenreDTO>>(GenreList);
            return Ok(GenreDTOList);
        }
    }
}
