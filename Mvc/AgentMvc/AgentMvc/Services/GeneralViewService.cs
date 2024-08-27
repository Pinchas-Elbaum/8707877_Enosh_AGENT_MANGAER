using AgentMvc.ViewModel;
using System.Reflection;

namespace AgentMvc.Services
{
    public class GeneralViewService(TargetService targetService, MissionService missionService, AgentService agentService)
    {
        
        /*public async Task<GenralView> CreateGeneralView()
        {
           

            GenralView genralView = new GenralView()
            {
                SumAgents = await agentService.GetAgents().Count(),
                
            }
        }
        */
    }
}
