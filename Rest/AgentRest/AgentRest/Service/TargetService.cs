using AgentRest.Dto;
using AgentRest.Models;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Service
{
    public class TargetService(ApplicationDbContext context, ImissionService mission) : ITargetService
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
        public async Task<Dictionary<string,int?>> CreateTargetAsync(TargetDto target)
        {
            TargetModel newTarget = new TargetModel();
            newTarget.Name = target.Name;
            newTarget.position = target.Position;
            newTarget.Image = target.PhotoUrl;

            await context.AddAsync(newTarget);
            await context.SaveChangesAsync();
            Dictionary<string, int?> dict = new() { { "id", newTarget.Id } };
            return dict;  
        }

        public async Task<TargetModel?> DeleteTargetAsync(int id)
        {
            var res = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);
            if (res == null)
            {
                return null;
            }
            context.Remove(res);
            await context.SaveChangesAsync();
            return res;
        }

        public async Task<List<TargetModel?>> GetAllTargetsAsync()
        {
            return await context.Targets.ToListAsync();
        }

        public async Task<TargetModel?> GetTargetAsync(int id)
        {
            return await context.Targets.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TargetModel?> Move(int id, DirectionDto direction)
        {     
            var target = await GetTargetAsync(id);
            if (target == null) {
                return null;
            }
            var newX = target.X + location[direction.Direction].Item1;
            var newY = target.Y + location[direction.Direction].Item2;

            if (newX > 1000 || newY > 1000 || newX < 0 || newY < 0)
            {
                throw new InvalidOperationException($"cannot move out of range ({target.X} {target.Y}");
            }
            target.X = newX;
            target.Y = newY;

            await context.SaveChangesAsync();
            mission.OfferToOrder();
            return target;      
        }

        public async Task<TargetModel?> SetPrimaryLocation(int id, PinDto pin)
        {
            TargetModel newTarget = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);
            if (newTarget == null)
            {
                return null;
            }
            newTarget.X = pin.x;
            newTarget.Y = pin.y;    
            await context.SaveChangesAsync();
            mission.OfferToOrder();
            return newTarget;
        }
    }
}
