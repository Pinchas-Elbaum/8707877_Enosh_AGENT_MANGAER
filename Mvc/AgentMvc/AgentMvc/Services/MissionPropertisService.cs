using AgentMvc.ViewModel;
using System.Reflection;
using System.Text.Json;

namespace AgentMvc.Services
{
    public class MissionPropertisService(TargetService targetService, MissionService missionService, AgentService agentService)
    {
        public async Task<List<MissionsPropertis>?> GetMissionsProperties()
        {
            var Missons = await missionService.GetMissions();
            var agents = await agentService.GetAgents();
            var targets = await targetService.GetTargets();



            List<MissionsPropertis> missionProperties = new();
            foreach (var mission in Missons)
            {
                MissionsPropertis Mp = new()
                {
                    id = mission.Id,
                    TimeLeft = mission.TimeLeft,
                    Distance = mission.TimeLeft * 5,

                    NickName =  agents.FirstOrDefault(a => a.Id == mission.AgentId).NickName,
                    AgentX = agents.FirstOrDefault(a => a.Id == mission.AgentId).X,
                    AgentY = agents.FirstOrDefault(a => a.Id == mission.AgentId).Y,

                    TargetName = targets.FirstOrDefault(t => t.Id == mission.TargetId).Name,
                    TargetX = agents.FirstOrDefault(t => t.Id == mission.TargetId).X,
                    TargetY = agents.FirstOrDefault(t => t.Id == mission.TargetId).Y
                };
                missionProperties.Add(Mp);
            }
            if (missionProperties.Count > 0)
            {
                return missionProperties;
            }
            return null;
        }
        
    }
}
