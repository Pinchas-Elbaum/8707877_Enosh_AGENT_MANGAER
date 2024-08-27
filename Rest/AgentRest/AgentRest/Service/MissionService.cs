using AgentRest.Models;
using AgentsApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AgentRest.Service
{
    public class MissionService(ApplicationDbContext context, IServiceProvider serviceProvider) : ImissionService
    {
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();
        public double CalculateTimeToHit(TargetModel target, AgentModel agent)
        {
            double distance = GetDistance(target, agent);
            var timeToHit = distance / 5;
            return timeToHit;
        }

        public async Task<List<MissionModel?>> GetAllMissionsAsync()
        {
            return await context.Missions.ToListAsync();
        }

        public double GetDistance(TargetModel target, AgentModel agent)
        {
            int AgentX = agent.X;
            int AgentY = agent.Y;

            int TargetX = target.X;
            int TargetY = target.Y;

            double distance = Math.Sqrt(Math.Pow(AgentX - TargetX, 2) + Math.Pow(AgentY - TargetY, 2));
            return distance;
        }

        public async Task<MissionModel?> GetMission(int id)
        {
            var mission = await context.Missions.FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null)
            {
                return null;    
            }
            return mission;
        }

        public async Task OfferToOrder()
        {
            var targets = await context.Targets.Where(t => t.Status == TargetStatus.Live).ToListAsync();
            var agents = await context.Agents.Where(a => a.Status == AgentStatus.Dormant).ToListAsync();
            foreach (var target in targets)
            {
                foreach (var agent in agents)
                {
                    if (GetDistance(target, agent) <= 200)
                    {
                        var mission = new MissionModel()
                        {
                            AgentId = agent.Id,
                            TargetId = target.Id,
                            Status = 0
                        };
                        context.Missions.Add(mission);
                        context.SaveChangesAsync();
                    }
                }
            }
        }

        

        public async Task UpdateMissionsAsync()
        {
            var missions = await context.Missions.Where(m => m.Status == MissionStatus.LinkedToTask).ToListAsync();

            foreach (var mission in missions)
            {
                var agent = await agentService.GetAgentAsync(mission.AgentId);
                var target = await targetService.GetTargetAsync(mission.TargetId);
                var distance = GetDistance(target, agent);
                if (distance == 0)
                {
                    
                    agent.Status = 0;
                    mission.Status = MissionStatus.Done;
                    mission.ActualExecutionTime = DateTime.Now;
                    target.Status = TargetStatus.Eliminated;
                    await context.SaveChangesAsync();
                }
                if (target.X > agent.X)
                {
                    agent.X += 1;
                    await context.SaveChangesAsync();
                }
                if (target.X < agent.X)
                {
                    agent.X -= 1;
                    await context.SaveChangesAsync();
                }
                if (target.Y > agent.Y)
                {
                    agent.Y += 1;
                    await context.SaveChangesAsync();
                }
                if (target.Y < agent.Y)
                {
                    agent.Y -= 1;
                    await context.SaveChangesAsync();
                }
                mission.TimeLeft = CalculateTimeToHit(target, agent);
                await context.SaveChangesAsync();
            }

        }

        public async Task<MissionModel?> UpdateStatus(int missionId)
        {
            var model = await GetMission(missionId);  
            
            if (model == null)
            {
                return null;
            }
            var agent = await agentService.GetAgentAsync(model.AgentId);
            var target = await targetService.GetTargetAsync(model.TargetId);

            if (agent.Status == AgentStatus.Activity || target.Status == TargetStatus.Eliminated || target.Status == TargetStatus.Persecuted)
            {
                return null;
            }

            model.Status = MissionStatus.LinkedToTask;
            agent.Status = AgentStatus.Activity;
            target.Status = TargetStatus.Persecuted;
            model.TimeLeft = GetDistance(target, agent);
            await context.SaveChangesAsync();
            return model;
            
        }

        
    }
}
