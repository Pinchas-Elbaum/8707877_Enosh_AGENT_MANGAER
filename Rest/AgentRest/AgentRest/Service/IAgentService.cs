using AgentRest.Models;

namespace AgentRest.Service
{
    public interface IAgentService
    {
        Task<AgentModel?> CreateAgentAsync(AgentModel agent);
        Task<AgentModel?> GetAgentAsync(int id);
        Task<AgentModel?> DeleteAgentAsync(int id);
    }
}
