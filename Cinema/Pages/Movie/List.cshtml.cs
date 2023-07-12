using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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

            dynamic dataObj = JsonConvert.DeserializeObject(strData);
            string data = dataObj.data.ToString();
            Movies = System.Text.Json.JsonSerializer.Deserialize<List<MovieDTO>>(data, options);

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

