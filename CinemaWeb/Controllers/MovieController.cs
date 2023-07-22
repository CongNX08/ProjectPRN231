using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWeb.Controllers
{
    public class MovieController : Controller
    {
        private readonly HttpClient client = null;
        private string MovieUrl = "";
        public MovieDTO Movie { get; set; }

        public MovieController()
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MovieUrl = "https://localhost:7052/api/Movie";
        }
        public async Task<IActionResult> List(int pageNumber = 0, int pageSize = 10)
        {
            
            
            string url = $"{MovieUrl}?pageSize={pageSize}&pageNumber={pageNumber}";
            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<MovieDTO> list = JsonSerializer.Deserialize<List<MovieDTO>>(strData, options);
            ViewData["listMovie"] = list;
            return View(list);

        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MovieDTO Movie)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(MovieUrl, Movie);
            response.EnsureSuccessStatusCode();
            return Redirect("/Movie/List");
        }
        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string url = $"{MovieUrl}/{id}";
            Console.WriteLine(id);

            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(strData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MovieDTO Movie = JsonSerializer.Deserialize<MovieDTO>(strData, options);
            Console.WriteLine(Movie);
            return View(Movie);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieDTO Movie)
        {
            string url = $"{MovieUrl}?id={id}";
            HttpResponseMessage response = await client.PutAsJsonAsync(url, Movie);
            return Redirect("/Movie/List");
        }

        //Delete
        [HttpGet]
        //'https://localhost:7052/api/Movie?id=26' \
        public async Task<IActionResult> Delete(int id)
        {
            string url = $"{MovieUrl}?id={id}";
            HttpResponseMessage response = await client.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                ViewData["Err"] = "Delete Fail!!!!";
            }
            return Redirect("/Movie/List");
        }
    }
}

