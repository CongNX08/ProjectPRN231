using BCrypt.Net;
using BusinessObject.Models;
using CinemaWebAPI.Config;
using CinemaWebAPI.Request.Auth;
using CinemaWebAPI.Response;
using CinemaWebAPI.Response.Auth;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private PersonRepository repository;
        private IConfiguration configuration;

        public AuthController(PersonRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                AuthResponse response = new AuthResponse();
                Person person = await repository.GetOneAsync(p => p.Email.Equals(request.Email));
                if (person == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = "User đăng nhập không tồn tại";
                    return NotFound(response);
                }
                if (!person.IsActive.Value)
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = "User bị khoá";
                    return Unauthorized(response);
                }
                bool IsPasswordMatch = BCrypt.Net.BCrypt.Verify(request.Password, person.Password);
                if (IsPasswordMatch)
                {
                    var mapper = AutoMapperConfig.InitializeAutomapper<Person, UserDTO>();
                    UserDTO user = mapper.Map<UserDTO>(person);
                    string token = GenerateToken(user);
                    if (string.IsNullOrEmpty(token))
                    {
                        response.IsSuccess = false;
                        response.ErrorMessages = "Cannot get token because null!";
                        return Unauthorized(response);
                    }
                    response.IsSuccess = true;
                    response.token = token;
                    response.Result = user;
                    return Ok(response);
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = "Sai mật khẩu";
                    return Unauthorized(response);
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                CommonResponse response = new CommonResponse();
                if (!ModelState.IsValid)
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = "Input không hợp lệ";
                    return BadRequest(response);
                }
                Person p = await repository.GetOneAsync(p => p.Email.Equals(request.Email));
                if (p != null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = "Tài khoản đã có người đăng kí";
                    return BadRequest(response);
                }
                var mapper = AutoMapperConfig.InitializeAutomapper<UserRegisterRequest, Person>();
                Person person = mapper.Map<Person>(request);
                person.IsActive = true;
                person.Type = Constants.UserType.NORMAL_USER;
                person.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                await repository.CreateAsync(person);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        private string GenerateToken(UserDTO user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
            (configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.PersonId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Fullname),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, GetRoleString(user.Type.Value)),
                new Claim("type", user.Type.Value.ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
        private string GetRoleString(int RoleId)
        {
            if (RoleId == Constants.UserType.ADMIN)
            {
                return "ADMIN";
            }
            if (RoleId == Constants.UserType.NORMAL_USER)
            {
                return "NORMAL_USER";
            }
            return "";
        }
    }
}
