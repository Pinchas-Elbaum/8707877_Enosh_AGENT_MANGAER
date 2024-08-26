using AgentRest.Models;
using AgentRest.Dto;

namespace AgentRest.Service
{
    public interface IAgentService
    {
        Task<Dictionary<string, int?>> CreateAgentAsync(AgentDto agent);
        Task<AgentModel?> GetAgentAsync(int id);
        Task<List<AgentModel?>> GetAllAgentsAsync();
        Task<AgentModel?> SetPrimaryLocation(int id, PinDto pin);
        Task< Dictionary<string, int>?> Move(int id, DirectionDto direction);
    }
}
