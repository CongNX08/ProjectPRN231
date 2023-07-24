using CinemaWebAPI.Request.Rate;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWeb.Controllers
{
    public class RateController : Controller
    {
        private readonly HttpClient client = null;
        private string RateUrl = "";
        public RateDTO Rate { get; set; }


        public RateController()
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            RateUrl = "https://localhost:7052/api/Rate";
        }


        //Manage
        [HttpGet]
        public async Task<IActionResult> Manage(int MovieId)
        {
            string MovieUrl = "https://localhost:7052/api/Movie";
            string urlMovie = $"{MovieUrl}/{MovieId}";
            HttpResponseMessage response1 = await client.GetAsync(urlMovie);
            string strData1 = await response1.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MovieDTO movie = System.Text.Json.JsonSerializer.Deserialize<MovieDTO>(strData1, options1);
            ViewBag.Movie = movie;

            // https://localhost:7052/api/Rate/14
            List<RateDTO> rate;
            string url = $"{RateUrl}/{MovieId}";
            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View();
            }
            dynamic dataObj = JsonConvert.DeserializeObject(strData);
            string data = dataObj.result.ToString();
            rate = System.Text.Json.JsonSerializer.Deserialize<List<RateDTO>>(data, options);
            return View(rate);
        }


        [HttpPost]
        public async Task<IActionResult> Manage(int id, MovieDTO Movie)
        {

            string url = $"{RateUrl}/{id}";
            HttpResponseMessage response = await client.PutAsJsonAsync(url, Movie);
            return Redirect("/Movie/List");
        }

        //Delete
        [HttpGet]
        //'https://localhost:7052/api/Movie?id=26' \
        public async Task<IActionResult> Delete(int movieID, int personID)
        {
            string url = $"{RateUrl}/delete";
            string token = HttpContext.Session.GetString("Token");
            //Build request body
            DeleteRateRequest requestData = new DeleteRateRequest();
            requestData.MovieId = movieID;
            requestData.PersonId = personID;
            //Build request gửi lên server
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url),
                Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
            };
            //Thêm header Authorization để xác thực user
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                ViewData["Err"] = "Delete Fail!!!!";
            }
            return Redirect($"/Rate/Manage?MovieId={movieID}");
        }

        //Manage
        [HttpGet]
        public async Task<IActionResult> Comment(int MovieId)
        {
            string MovieUrl = "https://localhost:7052/api/Movie";
            string urlMovie = $"{MovieUrl}/{MovieId}";
            HttpResponseMessage response1 = await client.GetAsync(urlMovie);
            string strData1 = await response1.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MovieDTO movie = System.Text.Json.JsonSerializer.Deserialize<MovieDTO>(strData1, options1);
            ViewBag.Movie = movie;

            // https://localhost:7052/api/Rate/14
            List<RateDTO> rate;
            string url = $"{RateUrl}/{MovieId}";
            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View();
            }
            dynamic dataObj = JsonConvert.DeserializeObject(strData);
            string data = dataObj.result.ToString();
            rate = System.Text.Json.JsonSerializer.Deserialize<List<RateDTO>>(data, options);
            return View(rate);
        }


        [HttpPost]
        public async Task<IActionResult> Comment(int movieID, int personID, string comment)
        {
            try
            {
                AddRateRequest addRateRequest = new AddRateRequest();
                addRateRequest.Comment = comment;
                addRateRequest.MovieId = movieID;
                addRateRequest.PersonId = personID;
                string token = HttpContext.Session.GetString("Token");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{RateUrl}/add"),
                    Content = new StringContent(JsonConvert.SerializeObject(addRateRequest), Encoding.UTF8, "application/json")
                };
                //Thêm header Authorization để xác thực user
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var resp = client.Send(request);
                dynamic json = JObject.Parse(resp.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                if (resp.IsSuccessStatusCode)
                {
                    return Redirect($"/Rate/Comment?MovieId={movieID}");
                }
                return Redirect($"/Rate/Comment?MovieId={movieID}");
            }
            catch (Exception ex)
            {

            }
            return Redirect($"/Rate/Comment?MovieId={movieID}");
        }

    }
}
