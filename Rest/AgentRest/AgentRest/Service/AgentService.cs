using AgentRest.Models;
using AgentRest.Dto;


using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {
        public Dictionary<string, Tuple<int, int>> location = new()
        {
            {"e", new (0, -1) },
            {"w", new (0, 1) },
            {"s", new (1, 0) },
            {"n", new (-1, 0) },
            {"se", new (-1, 1) },
            {"ne", new (-1, -1) },
            {"sw", new (1, 1) },
            {"nw", new (-1, 1) }
        };


        public async Task<int?> CreateAgentAsync(AgentDto agent)
        {
            AgentModel newAgent = new AgentModel();
            newAgent.NickName = agent.Nickname;
            newAgent.Image = agent.Photo_url;

            await context.AddAsync(newAgent);
            await context.SaveChangesAsync();
            return newAgent.Id;
        }


        public async Task<AgentModel?> GetAgentAsync(int id)
        {
            return await context.Agents.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<AgentModel?>> GetAllAgentsAsync()
        {
            return await context.Agents.ToListAsync();
        }


        public async Task<AgentModel?> SetPrimaryLocation(int id, PinDto pin)
        {
            AgentModel newAgent = await context.Agents.FirstOrDefaultAsync(x => x.Id == id);
            if (newAgent == null)
            {
                return null;
            }
            newAgent.X = pin.x;
            newAgent.Y = pin.y;
            await context.SaveChangesAsync();
            return newAgent;
        }


        public async Task<AgentModel?> Move(int id, DirectionDto direction)
        {
            var agent = await GetAgentAsync(id);
            agent.X += location[direction.Direction].Item1;
            agent.Y += location[direction.Direction].Item2;
            await context.SaveChangesAsync();
            return agent;
        }
    }
}
