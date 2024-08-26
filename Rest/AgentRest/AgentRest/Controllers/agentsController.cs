using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController(IAgentService agentService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetModel>> CreateTarget([FromBody] AgentDto agent)
        {
            try
            {
                var newAgent = await agentService.CreateAgentAsync(agent);
                
                return Created("", newAgent);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AgentModel>>> GetAllAgents()
        {
            return Ok(await agentService.GetAllAgentsAsync());
        }


        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> Update(int id, [FromBody] PinDto pin)
        {
            var res = await agentService.SetPrimaryLocation(id, pin);

            if (res == null)
            {
                return NotFound("no user to update");
            }
            return Ok(res);
        }


        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Dictionary<string, int>?>> Move(int id, [FromBody] DirectionDto direction)
        {
            var res = await agentService.Move(id, direction);

            try
            {
                if (res == null)
                {
                    return NotFound("no agent to move");
                }
                return Ok();
                   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
