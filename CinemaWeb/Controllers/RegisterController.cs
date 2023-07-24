using CinemaWebAPI.Request.Auth;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CinemaWeb.Controllers
{
    public class RegisterController : Controller
    {
        private static HttpClient httpClient;
        private static string baseApiUrl = "https://localhost:7052/api/Auth";
        public RegisterController()
        {
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterRequest registerRequest, string PasswordRepeat)
        {
            if (registerRequest != null && ModelState.IsValid)
            {
                try
                {
                    if (!String.IsNullOrEmpty(PasswordRepeat) && !PasswordRepeat.Equals(registerRequest.Password))
                    {
                        ViewBag.Message = "Mật khẩu không trùng khớp";
                        return View("Index");
                    }
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{baseApiUrl}/register"),
                        Content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json")
                    };
                    var resp = httpClient.Send(request);
                    dynamic json = JObject.Parse(resp.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    if (resp.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.Message = (string)json.errorMessages;
                    }
                    return View("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Đã có lỗi xảy ra khi đăng nhập";
                }
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index", "Register");
        }
    }
}
