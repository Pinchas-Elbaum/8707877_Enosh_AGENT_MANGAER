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
    public class targetsController(ITargetService targetService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetModel>> CreateTarget([FromBody] TargetDto target)
        {
            try
            {
                var newTarget = await targetService.CreateTargetAsync(target);
                return Created("", newTarget);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TargetModel>>> GetAllTargets()
        {
            return Ok(await targetService.GetAllTargetsAsync());
        }


        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> Update(int id, [FromBody] PinDto pin)
        {
            var res = await targetService.SetPrimaryLocation(id, pin);

            if (res == null)
            {
                return NotFound("no user to update");
            }
            return Ok(res);
        }


        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> Move(int id, [FromBody] DirectionDto direction)
        {
            var res = await targetService.Move(id, direction);

            if (res == null)
            {
                return NotFound("no target to move");
            }
            return Ok();
        }
    }
}
