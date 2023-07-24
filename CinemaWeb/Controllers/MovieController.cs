using CinemaWebAPI.Config;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private List<GenreDTO> genresList;
        string TitleSearchRES = "";



        public MovieController()
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MovieUrl = "https://localhost:7052/api/Movie";
        }

        private async Task LoadGenresList()
        {
            string genresUrl = "https://localhost:7052/api/Genre";
            HttpResponseMessage genresResponse = await client.GetAsync(genresUrl);
            string genresData = await genresResponse.Content.ReadAsStringAsync();
            var genresOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            genresList = JsonSerializer.Deserialize<List<GenreDTO>>(genresData, genresOptions);
        }


        public async Task<IActionResult> List(string? titleSearch, int? genreId, int pageNumber = 1, int pageSize = 3)
        {
            string username = HttpContext.Session.GetString("FullName");
            string usertype = HttpContext.Session.GetString("UserType");
            if (String.IsNullOrEmpty(username) || !usertype.Equals(Constants.UserType.ADMIN.ToString()))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            await LoadGenresList(); // Call the method to load genres list


            // Construct the API URL with titleSearch and genreId as query parameters
            string url = $"{MovieUrl}?titleSearch={titleSearch}&filterGenre={genreId}&pageSize={pageSize}&pageNumber={pageNumber}";

            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MovieDTO> list = JsonSerializer.Deserialize<List<MovieDTO>>(strData, options);
            if (titleSearch != null)
            {
                ViewBag.TitleSearchRES = titleSearch; // Set titleSearchRES to ViewBag
            }
            if (genreId != null)
            {
                ViewBag.genreId = genreId; // Set genreId to ViewBag
            }

            if (list != null && list.Count > 0)
            {
                ViewBag.ToTalPage = (int)Math.Ceiling((double)list[0].CountNumberofResult / pageSize);
            }
            else
            {

                ViewBag.TotalPage = 1;
            }
            ViewData["pageNumber"] = pageNumber;
            ViewData["pageSize"] = pageSize;
            ViewData["GenresList"] = new SelectList(genresList, "GenreId", "Description");
            return View(list);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string username = HttpContext.Session.GetString("FullName");
            string usertype = HttpContext.Session.GetString("UserType");
            if (String.IsNullOrEmpty(username) || !usertype.Equals(Constants.UserType.ADMIN.ToString()))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            await LoadGenresList(); // Call the method to load genres list

            // Pass the GenresList to the view
            ViewData["GenresList"] = new SelectList(genresList, "GenreId", "Description");
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
            string username = HttpContext.Session.GetString("FullName");
            string usertype = HttpContext.Session.GetString("UserType");
            if (String.IsNullOrEmpty(username) || !usertype.Equals(Constants.UserType.ADMIN.ToString()))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            await LoadGenresList(); // Call the method to load genres list

            string url = $"{MovieUrl}/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MovieDTO movie = JsonSerializer.Deserialize<MovieDTO>(strData, options);

            // Set the selected genre ID in ViewBag
            ViewBag.SelectedGenreId = movie.GenreId;

            ViewData["GenresList"] = new SelectList(genresList, "GenreId", "Description");
            return View(movie);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieDTO Movie)
        {
            string username = HttpContext.Session.GetString("FullName");
            string usertype = HttpContext.Session.GetString("UserType");
            if (String.IsNullOrEmpty(username) || !usertype.Equals(Constants.UserType.ADMIN.ToString()))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            //'https://localhost:7052/api/Movie/1045' \
            string url = $"{MovieUrl}/{id}";
            HttpResponseMessage response = await client.PutAsJsonAsync(url, Movie);
            return Redirect("/Movie/List");
        }

        //Delete
        [HttpGet]
        //'https://localhost:7052/api/Movie?id=26' \
        public async Task<IActionResult> Delete(int id)
        {
            string username = HttpContext.Session.GetString("FullName");
            string usertype = HttpContext.Session.GetString("UserType");
            if (String.IsNullOrEmpty(username) || !usertype.Equals(Constants.UserType.ADMIN.ToString()))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
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