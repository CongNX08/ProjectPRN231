using System.ComponentModel.DataAnnotations;

namespace CinemaWebAPI.Request.Auth
{
    public class UserRegisterRequest
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
