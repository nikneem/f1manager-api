using System;
using System.Threading.Tasks;
using F1Manager.Admin.ActualTeams.Abstractions;
using F1Manager.Admin.ActualTeams.DataTransferObjects;
using F1Manager.Api.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActualTeamsController : F1ManagerApiControllerBase
    {
        private readonly IActualTeamsService _actualTeamsService;

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var actualTeams = await _actualTeamsService.GetActive();
            return Ok(actualTeams);
        }


        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ActualTeamListFilterDto filter)
        {
            var actualTeams = await _actualTeamsService.List(filter);
            return Ok(actualTeams);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Single(Guid id)
        {
            var dto = await _actualTeamsService.Get(id);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActualTeamDetailsDto dto)
        {
            var createdDriver = await _actualTeamsService.Create(dto);
            return createdDriver != null ? Ok(createdDriver) : BadRequest();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ActualTeamDetailsDto dto)
        {
            var updatedDriver = await _actualTeamsService.Update(id, dto);
            return updatedDriver != null ? Ok(updatedDriver) : BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedDriver = await _actualTeamsService.Delete(id);
            return deletedDriver ? Ok() : BadRequest();
        }

        [HttpDelete("{id:guid}/undelete")]
        public async Task<IActionResult> Undelete(Guid id)
        {
            var undeletedDriver = await _actualTeamsService.Undelete(id);
            return undeletedDriver ? Ok() : BadRequest();
        }



        public ActualTeamsController(IActualTeamsService actualTeamsService)
        {
            _actualTeamsService = actualTeamsService;
        }
    }
}
