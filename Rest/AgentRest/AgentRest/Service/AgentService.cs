using AgentRest.Models;
using AgentRest.Dto;


using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Service
{
    public class AgentService(ApplicationDbContext context, ImissionService mission) : IAgentService
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


        public async Task<Dictionary<string, int?>> CreateAgentAsync(AgentDto agent)
        {
            AgentModel newAgent = new AgentModel();
            newAgent.NickName = agent.Nickname;
            newAgent.Image = agent.PhotoUrl;

            await context.AddAsync(newAgent);
            await context.SaveChangesAsync();
            Dictionary<string, int?> dict = new() { { "id", newAgent.Id } };
            return dict;
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
            mission.OfferToOrder();
            return newAgent;
        }


        public async Task<AgentModel?> Move(int id, DirectionDto direction)
        {
            var agent = await GetAgentAsync(id);
            if (agent == null)
            {
                return null;
            }

            if (agent.Status == AgentStatus.Activity)
            {
                return null;
                throw new InvalidOperationException("Status Agent: Activity");
                  
            }
            var newX = agent.X + location[direction.Direction].Item1;
            var newY = agent.Y + location[direction.Direction].Item2;

            if (newX > 1000 || newY > 1000 || newX < 0 || newY < 0)
            {
                throw new InvalidOperationException($"cannot move out of range ({agent.X} {agent.Y}");  
            }
            agent.X = newX;
            agent.Y = newY;
            

            await context.SaveChangesAsync();
            await mission.OfferToOrder();
            return agent;
            
        }
    }
}
