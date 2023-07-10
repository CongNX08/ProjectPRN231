using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Cinema.Pages.Movie
{
    public class ListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private string MovieUrl = "https://localhost:7052/api/Movie";
        public ListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<MovieDTO> Movies { get; set; } = new List<MovieDTO>();
       

        public async Task OnGet()
        {
            var response = await _httpClient.GetAsync(MovieUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Movies = JsonSerializer.Deserialize<List<MovieDTO>>(strData, options);   
            

        }

        public async Task<IActionResult> OnPost(int deleteID)
        {
            string url = $"{MovieUrl}?id={deleteID}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Movie/List");
            }
            else
            {
                ViewData["Err"] = "Delete Fail!!!!";
                return Page();
            }
        }
    }
}

