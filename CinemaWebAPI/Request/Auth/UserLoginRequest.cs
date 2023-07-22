using System.ComponentModel.DataAnnotations;

namespace CinemaWebAPI.Request.Auth
{
    public class UserLoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
