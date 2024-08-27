using AgentMvc.ViewModel;
using System.Text.Json;

namespace AgentMvc.Services
{
    public class TargetService(IHttpClientFactory clientFactory)
    {
        private readonly string baseUrl = "https://localhost:7116/targets";
        public async Task<List<TargetModel>?> GetTargets()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result != null)
            {
                var content = await result.Content.ReadAsStringAsync();

                List<TargetModel>? targets = JsonSerializer.Deserialize<List<TargetModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                
                return targets;
            }

            return null;
        }
    }
}
