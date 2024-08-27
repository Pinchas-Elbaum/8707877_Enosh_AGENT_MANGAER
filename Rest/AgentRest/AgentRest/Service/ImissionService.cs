using AgentRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Service
{
    public interface ImissionService
    {
        double GetDistance(TargetModel target, AgentModel agent);
        Task OfferToOrder();
        Task UpdateMissionsAsync();
        
        double CalculateTimeToHit(TargetModel target, AgentModel agent);
        Task<List<MissionModel?>> GetAllMissionsAsync();
        Task<MissionModel?> UpdateStatus(int id);
        Task<MissionModel?> GetMission(int id);


    }
}
