using AgentRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Service
{
    public interface ImissionService
    {
        double GetDistance(TargetModel target, AgentModel agent);
        void OfferToOrder();
        Task UpdateMissionsAsync();
        Task<MissionModel> Update(int missionId);
        double CalculateTimeToHit(TargetModel target, AgentModel agent);
        Task<List<MissionModel?>> GetAllMissionsAsync();
        Task<MissionModel?> UpdateStatus(int id, int status);
        Task<MissionModel?> GetMission(int id);


    }
}
