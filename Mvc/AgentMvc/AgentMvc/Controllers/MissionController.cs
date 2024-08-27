using AgentMvc.Services;
using AgentMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace AgentMvc.Controllers
{
    public class MissionController(IHttpClientFactory clientFactory, MissionPropertisService missionPropertis) : Controller
    {
        private readonly string baseUrl = "https://localhost:7116/missions";
        public async Task<IActionResult> Index()
        {
            var mp = await missionPropertis.GetMissionsProperties();
            if (mp != null)
            {  
                return View(mp);
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

