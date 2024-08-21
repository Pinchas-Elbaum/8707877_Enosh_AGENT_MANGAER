using AgentRest.Models;


using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {
        public async Task<AgentModel?> CreateAgentAsync(AgentModel agent)
        {
            await context.AddAsync(agent);
            await context.SaveChangesAsync();
            return agent;
        }

        public async Task<AgentModel?> DeleteAgentAsync(int id)
        {
            var res = await context.Agents.FirstOrDefaultAsync(x => x.Id == id);
            if (res == null)
            {
                return null;
            }
            context.Remove(res);
            await context.SaveChangesAsync();
            return res;
        }

        public async Task<AgentModel?> GetAgentAsync(int id)
        {
            return await context.Agents.FirstOrDefaultAsync(x => x.Id == id); 
        }
    }
}
