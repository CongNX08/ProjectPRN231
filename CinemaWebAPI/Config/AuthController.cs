using BCrypt.Net;
using BusinessObject.Models;
using CinemaWebAPI.Request.Auth;
using CinemaWebAPI.Response.Auth;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaWebAPI.Config
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
                bool IsPasswordMatch = BCrypt.Net.BCrypt.Verify(request.Password, person.Password);
                if (IsPasswordMatch)
                {
                    var mapper = AutoMapperConfig.InitializeAutomapper<Person, UserDTO>();
                    UserDTO user = mapper.Map<UserDTO>(person);
                    string token = GenerateToken(user);
                    if (String.IsNullOrEmpty(token))
                    {
                        response.IsSuccess = false;
                        response.ErrorMessages = "Cannot get token because null!";
                        return BadRequest(response);
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
                    return BadRequest(response);
                }
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
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Fullname),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
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
    }
}
