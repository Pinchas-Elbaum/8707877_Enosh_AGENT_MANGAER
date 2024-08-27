using AgentMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace AgentMvc.Controllers
{
    public class MissionController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string baseUrl = "https://localhost:7116/missions";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                List<MissionModel>? missions = JsonSerializer.Deserialize<List<MissionModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var suggestionMissions = missions.Where(m => m.Status == MissionStatus.Suggestion).ToList();
                return View(suggestionMissions);
            }
            return RedirectToAction("Index", "Home");

        }
       
        
        public async Task<IActionResult> Team(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/{id}");
           

            var result = await httpClient.SendAsync(request);
            
            return RedirectToAction("Index");
        }
    }
}
