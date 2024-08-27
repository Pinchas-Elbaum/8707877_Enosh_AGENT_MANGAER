using AgentMvc.ViewModel;
using System.Text.Json;

namespace AgentMvc.Services
{
    public class AgentService(IHttpClientFactory clientFactory)
    {
        private readonly string baseUrl = "https://localhost:7116/agents";
        public async Task<List<AgentModel>?> GetAgents()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result != null)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<AgentModel>? agents = JsonSerializer.Deserialize<List<AgentModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });     
                return agents;
            }
            return null;
        }
        public async Task<AgentModel?> GetAgentById(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result != null)
            {
                var content = await result.Content.ReadAsStringAsync();
                AgentModel? agent = JsonSerializer.Deserialize<AgentModel?>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return agent;
            }
            return null;
        }
    }
}
