using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // //////////////////////////Get All data
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {

            IEnumerable<Movie> MovieList = await _dbMovie.GetAllAsync(null, x => x.Genre);
            IEnumerable<MovieDTO> MovieDTOList = _mapper.Map<IEnumerable<MovieDTO>>(MovieList);
            return Ok(MovieDTOList);
        }


        // ////////////////////////Get One data
        [HttpGet("{Movie_id:int}", Name = "GetOneMovie")] // notedasssssssssssss
        public async Task<ActionResult<MovieDTO>> GetOneMovie(int Movie_id)
        {

            if (Movie_id == 0)
            {

                return BadRequest();
            }
            var Movie = await _dbMovie.GetOneAsync(x => x.MovieId == Movie_id, x => x.Genre);
            if (Movie == null)
            {

                return NotFound();
            }

            return Ok(_mapper.Map<MovieDTO>(Movie));
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
        public async Task<ActionResult<MovieDTO>> UpdateMovie(int id, [FromBody] MovieDTO MovieUpdate)
        {

            if (MovieUpdate == null || id != MovieUpdate.MovieId)
            {
                return BadRequest();
            }


            ////////////////////// dung AutoMapper/////////
            Movie newUpdateMovie = _mapper.Map<Movie>(MovieUpdate);

            await _dbMovie.UpdateAsync(newUpdateMovie);
            return Ok();
        }


        //////////////// DELETE/////////////////////////
        [HttpDelete( Name = "DeleteMovie")]
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
