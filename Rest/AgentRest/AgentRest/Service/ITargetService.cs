using AgentRest.Models;
using AgentRest.Dto;

namespace AgentRest.Service
{
    public interface ITargetService
    {
        Task<int?> CreateTargetAsync(TargetDto target);
        Task<TargetModel?> GetTargetAsync(int id);
        Task<TargetModel?> DeleteTargetAsync(int id);
        Task<List<TargetModel?>> GetAllTargetsAsync();
        Task<TargetModel?> SetPrimaryLocation(int id, PinDto pin);
        Task<TargetModel?> Move(int id, DirectionDto direction);
    }
}
