using BusinessObject.Models;
using CinemaWebAPI.Config;
using CinemaWebAPI.Request.Rate;
using CinemaWebAPI.Response;
using CinemaWebAPI.Response.Rate;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RateController : ControllerBase
    {
        private RateRepository repository;

        public RateController(RateRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("{MovieId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<RateDTO>>> GetAllRatesByMovieId(int MovieId)
        {
            try
            {
                Expression<Func<Rate, object>>[] includes = { r => r.Movie, r => r.Person };
                List<Rate> rates = await repository.GetAllAsync(r => r.MovieId == MovieId, includes);
                RateListResponse response = new RateListResponse();
                if (MovieId == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
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

        [HttpPost("add")]
        public async Task<ActionResult<CommonResponse>> AddRate([FromBody] AddRateRequest addRateRequest)
        {
            try
            {
                CommonResponse response = new CommonResponse();
                if (!ModelState.IsValid)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                //Kiểm tra chỉ user hiện tại được phép add comment cho chính mình, không cho phép add cho người khác
                string CurrentUserId = User.FindFirstValue("Id");
                if (String.IsNullOrEmpty(CurrentUserId) || Int32.Parse(CurrentUserId) != addRateRequest.PersonId)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.ErrorMessages = "Request không hợp lệ";
                    return BadRequest(response);
                }
                //Kiểm tra nếu comment đã tồn tại trong film rồi thì không cho phép user đó add thêm comment nữa
                Rate rate = await repository.GetOneAsync(r => r.MovieId == addRateRequest.MovieId && r.PersonId == addRateRequest.PersonId, null);
                if (rate != null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.ErrorMessages = "User đã comment vào bộ phim này!";
                    return BadRequest(response);
                }
                var mapper = AutoMapperConfig.InitializeAutomapper<AddRateRequest, Rate>();
                Rate RateToAdd = mapper.Map<Rate>(addRateRequest);
                RateToAdd.Time = DateTime.Now;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = addRateRequest;
                await repository.CreateAsync(RateToAdd);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPut("edit")]
        public async Task<ActionResult<CommonResponse>> EditRate([FromBody] AddRateRequest addRateRequest)
        {
            try
            {
                CommonResponse response = new CommonResponse();
                if (!ModelState.IsValid)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                //Kiểm tra chỉ cho phép user đăng nhập hiện tại được phép edit comment của mình
                string CurrentUserId = User.FindFirstValue("Id");
                if (String.IsNullOrEmpty(CurrentUserId) || Int32.Parse(CurrentUserId) != addRateRequest.PersonId)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.ErrorMessages = "Request không hợp lệ";
                    return BadRequest(response);
                }
                //Lấy ra comment cần update
                Rate rate = await repository.GetOneAsync(r => r.MovieId == addRateRequest.MovieId && r.PersonId == addRateRequest.PersonId, null);
                if (rate == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                rate.Time = DateTime.Now;
                rate.Comment = addRateRequest.Comment;
                rate.NumericRating = addRateRequest.NumericRating;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = addRateRequest;
                await repository.UpdateAsync(rate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<CommonResponse>> DeleteRate([FromBody] DeleteRateRequest deleteRateRequest)
        {
            try
            {
                CommonResponse response = new CommonResponse();
                if (!ModelState.IsValid)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                //Kiểm tra chỉ cho phép user đăng nhập hiện tại được phép xoá comment của mình
                string CurrentUserId = User.FindFirstValue("Id");
                string UserType = User.FindFirstValue("type");
                if ((String.IsNullOrEmpty(CurrentUserId) || Int32.Parse(CurrentUserId) != deleteRateRequest.PersonId)
                    && Int32.Parse(UserType) == Constants.UserType.NORMAL_USER)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.ErrorMessages = "Request không hợp lệ";
                    return BadRequest(response);
                }
                //Lấy ra comment cần xoá
                Rate rate = await repository.GetOneAsync(r => r.MovieId == deleteRateRequest.MovieId && r.PersonId == deleteRateRequest.PersonId, null);
                if (rate == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                await repository.RemoveAsync(rate);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = deleteRateRequest;

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
