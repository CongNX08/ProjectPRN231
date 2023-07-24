using CinemaWebAPI.Request.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using CinemaWeb.Models;
using Newtonsoft.Json.Linq;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CinemaWeb.Controllers
{
    public class LoginController : Controller
    {
        private static HttpClient httpClient;
        private static string baseApiUrl = "https://localhost:7052/api/Auth";
        public LoginController()
        {
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            ViewBag.Message = "";
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{baseApiUrl}/login"),
                        Content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json")
                    };
                    var resp = httpClient.Send(request);
                    dynamic json = JObject.Parse(resp.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    if (resp.IsSuccessStatusCode)
                    {
                        UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(json.result));
                        HttpContext.Session.SetString("UserId", userDTO.PersonId.ToString());
                        HttpContext.Session.SetString("FullName", userDTO.Fullname);
                        HttpContext.Session.SetString("UserType", userDTO.Type.ToString());
                        HttpContext.Session.SetString("Token", (string)json.token);
                        return RedirectToAction("Index", "Home");
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
                    return View("Index");
                }
            }
            ViewBag.Message = "Dữ liệu không hợp lệ";
            return View("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            if (HttpContext.Session.IsAvailable)
            {
                HttpContext.Session.Clear();
            }
            return View("Index");
        }
    }
}
