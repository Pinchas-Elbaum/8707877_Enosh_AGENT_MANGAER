using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Service;
using AgentsApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class missionsController(ImissionService mission, ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MissionModel>>> GetAllMission()
        {
            return Ok(await mission.GetAllMissionsAsync());
        }
        [HttpPost("update")]
        public async Task<ActionResult> UpdateMissions()
        {
            await mission.UpdateMissionsAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MissionModel>> Updatstatus(int id)
        {
            var res = await mission.UpdateStatus(id);
            if (res == null)
            {
                return NotFound("no target to Team");
            }
            return Ok(res);
        }

    }
}
