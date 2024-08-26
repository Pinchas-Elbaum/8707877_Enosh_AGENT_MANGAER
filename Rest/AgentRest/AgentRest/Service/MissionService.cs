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

        public async void OfferToOrder()
        {
            var targets = await context.Targets.Select(t => t).ToListAsync();
            var agents = await context.Agents.Select(a => a).ToListAsync();
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

        public async Task<MissionModel> Update(int missionId)
        {
            var mission = await GetMission(missionId);
            return mission;
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
                    context.Targets.Remove(target);
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

        public async Task<MissionModel?> UpdateStatus(int status, int missionId)
        {
            var model = await GetMission(missionId);  
            if (model == null)
            {

                return null;
            }
            switch (status)
            {
                case 0:
                    model.Status = MissionStatus.Suggestion;
                    break;
                case 1:
                    model.Status = MissionStatus.LinkedToTask;
                    break;
                case 2:
                    model.Status = MissionStatus.Done;
                    break;
            }
            context.SaveChangesAsync();
            return model;
            
        }

        
    }
}
