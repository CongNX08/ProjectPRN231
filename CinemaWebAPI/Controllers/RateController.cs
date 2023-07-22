using BusinessObject.Models;
using CinemaWebAPI.Config;
using CinemaWebAPI.Response.Rate;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private RateRepository repository;

        public RateController(RateRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("{MovieId}")]
        public async Task<ActionResult<List<RateDTO>>> GetAllRatesByMovieId(int MovieId)
        {
            try
            {
                Expression<Func<Rate, object>>[] includes = { r => r.Movie, r => r.Person };
                List<Rate> rates = await repository.GetAllAsync(r => r.MovieId == MovieId, includes);
                RateListResponse response = new RateListResponse();
                if (rates == null || rates.Count == 0)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return NotFound(response);
                }
                var mapper = AutoMapperConfig.InitializeAutomapper<Rate, RateDTO>();
                List<RateDTO> LstRateDto = new List<RateDTO>();
                foreach (var rate in rates)
                {
                    RateDTO rateDTO = mapper.Map<RateDTO>(rate);
                    rateDTO.PersonName = rate.Person.Fullname;
                    rateDTO.Time = rate.Time.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    LstRateDto.Add(rateDTO);
                }
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = LstRateDto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
