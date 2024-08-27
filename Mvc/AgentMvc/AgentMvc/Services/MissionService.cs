using AgentMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgentMvc.Services
{
    public class MissionService(IHttpClientFactory clientFactory)
    {
        private readonly string baseUrl = "https://localhost:7116/missions";
        public async Task<List<MissionModel>?> GetMissions()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result != null)
            {
                var content = await result.Content.ReadAsStringAsync();

                List<MissionModel>? missions = JsonSerializer.Deserialize<List<MissionModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var suggestionMissions = missions.Where(m => m.Status == MissionStatus.Suggestion).ToList();
                return suggestionMissions;
            }

            return null;
        }
    }
}
