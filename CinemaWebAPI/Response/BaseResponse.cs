using DataAccess.Entity;

namespace CinemaWebAPI.Response
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public Paging? Paging { get; set; }
    }
}
