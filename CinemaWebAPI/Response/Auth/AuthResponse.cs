namespace CinemaWebAPI.Response.Auth
{
    public class AuthResponse : BaseResponse<object>
    {
        public string? token { get; set; }
    }
}
