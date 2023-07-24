using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Entity;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly MovieRepository _dbMovie;
        private readonly IMapper _mapper;
        public MovieController(MovieRepository dbMovie, IMapper mapper)
        {
            _dbMovie = dbMovie;
            _mapper = mapper;

        }

        //////////////////////////Get All data

        [HttpGet]
        public async Task<ActionResult<MovieDTO>> GetMovies([FromQuery(Name = "filterGenre")] int? genreId,
            [FromQuery] string? titleSearch, [FromQuery] int? year, int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Movie> MovieList = await _dbMovie.GetAllAsync(includeProperties: "Genre", pageSize: pageSize, pageNumber: pageNumber);
            IEnumerable<Movie> MovieTotal = await _dbMovie.GetAllAsync(includeProperties: "Genre");

            if(genreId != null && !string.IsNullOrEmpty(titleSearch))
            {
                string search = titleSearch.ToLower();
                MovieList = await _dbMovie.GetAllAsync(u => u.GenreId == genreId && u.Title.ToLower().Contains(search), includeProperties: "Genre", pageSize: pageSize, pageNumber: pageNumber);
                MovieTotal = await _dbMovie.GetAllAsync(u => u.GenreId == genreId && u.Title.ToLower().Contains(search), includeProperties: "Genre");
            }
            else if(genreId != null)
            {
                MovieList = await _dbMovie.GetAllAsync(u => u.GenreId == genreId, includeProperties: "Genre", pageSize: pageSize, pageNumber: pageNumber);
                MovieTotal = await _dbMovie.GetAllAsync(u => u.GenreId == genreId, includeProperties: "Genre");

            }
            else if (!string.IsNullOrEmpty(titleSearch))
            {
                string search = titleSearch.ToLower();
                MovieList = await _dbMovie.GetAllAsync(u => u.Title.ToLower().Contains(search), includeProperties: "Genre", pageSize: pageSize, pageNumber: pageNumber);
                MovieTotal = await _dbMovie.GetAllAsync(u => u.Title.ToLower().Contains(search), includeProperties: "Genre");
            }
            if (year != null)
            {
                MovieList = await _dbMovie.GetAllAsync(u => u.Year == year, includeProperties: "Genre", pageSize: pageSize, pageNumber: pageNumber);
                MovieTotal = await _dbMovie.GetAllAsync(u => u.Year == year, includeProperties: "Genre");

            }

            List<MovieDTO> listDTO = _mapper.Map<List<MovieDTO>>(MovieList);
            List<MovieDTO> listDTO2 = _mapper.Map<List<MovieDTO>>(MovieTotal);

            listDTO.ForEach(dto => dto.CountNumberofResult =  listDTO2.Count);

            return Ok(listDTO);
        }




        ////////////////////////Get One data
        [HttpGet("{Movie_id:int}", Name = "GetOneMovie")]
        public async Task<ActionResult<MovieDTO>> GetOneMovie(int Movie_id)
        {

            if (Movie_id == 0)
            {

                return BadRequest();
            }
            var Movie = await _dbMovie.GetOneAsync(x => x.MovieId == Movie_id, includeProperties: "Genre");
            if (Movie == null)
            {

                return NotFound();
            }
            MovieDTO movieDTO = _mapper.Map<MovieDTO>(Movie);
            return Ok(movieDTO);
        }

        //////////////// Create/////////////////////
        [HttpPost]
        public async Task<ActionResult<MovieDTO>> CreateMovie([FromBody] MovieDTO MovieCreate)
        {

            if (await _dbMovie.GetOneAsync(x => x.Title.ToLower() == MovieCreate.Title.ToLower()) != null)
            {
                ModelState.AddModelError("customerError", "Movie already");
                return BadRequest(ModelState);
            }
            if (MovieCreate == null)
            {
                return BadRequest(MovieCreate);
            }

            ////////////////////// dung AutoMapper/////////
            //Movie newMovie = _mapper.Map<Movie>(MovieCreate);
            //await _dbMovie.CreateAsync(newMovie);

            //return CreatedAtRoute("GetOneMovie", new { Movie_id = newMovie.MovieId }, newMovie);       // Ok   

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            var newMovie = new Movie
            {
                Title = MovieCreate.Title,
                Year = MovieCreate.Year,
                Image = MovieCreate.Image,
                Description = MovieCreate.Description,
                GenreId = MovieCreate.GenreId,
                RatingPoint = MovieCreate.RatingPoint,
            };

            // Thực hiện tạo mới Movie
            await _dbMovie.CreateAsync(newMovie);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var createdMovieDto = _mapper.Map<MovieDTO>(newMovie);

            return CreatedAtRoute("GetOneMovie", new { Movie_id = createdMovieDto.MovieId }, createdMovieDto);
        }


        // CRUD_008 //////////Update///////////////////////
        [HttpPut("{Movie_id:int}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDTO>> UpdateMovie(int Movie_id, [FromBody] MovieDTO MovieUpdate)
        {

            if (MovieUpdate == null || Movie_id != MovieUpdate.MovieId)
            {
                return BadRequest();
            }
            Movie newUpdateMovie = _mapper.Map<Movie>(MovieUpdate);

            await _dbMovie.UpdateAsync(newUpdateMovie);
            return Ok();
        }

        //////////////// DELETE/////////////////////////
        [HttpDelete(Name = "DeleteMovie")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var Movie = await _dbMovie.GetOneAsync(x => x.MovieId == id);
            if (Movie == null)
            {
                return NotFound();
            }
            await _dbMovie.RemoveAsync(Movie);
            return NoContent();
        }
    }
}
