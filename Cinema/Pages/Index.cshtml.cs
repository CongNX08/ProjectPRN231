using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Cinema.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private string MovieUrl = "https://localhost:7052/api/Movie";
        public IndexModel(HttpClient httpClient)
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
    }
}