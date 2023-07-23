using DataAccess.Entity;
using System.Net;

namespace CinemaWebAPI.Response
{
    public class BaseResponse<T>
    {
        public HttpStatusCode? StatusCode { get; set; }
        public bool? IsSuccess { get; set; } = true;
        public string? ErrorMessages { get; set; }
        public T? Result { get; set; }
        public Paging? Paging { get; set; }
    }
}
