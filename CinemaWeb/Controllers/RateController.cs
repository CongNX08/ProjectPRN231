using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
        public async Task<IActionResult> Delete(int movieID, int personID )
        {
            string url = $"{RateUrl}?id={movieID}";
            HttpResponseMessage response = await client.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                ViewData["Err"] = "Delete Fail!!!!";
            }
            return Redirect("/Movie/List");
        }
    }
}
