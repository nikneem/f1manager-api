using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : F1ManagerApiControllerBase
    {
        private readonly ITeamsService _teamsService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            return team == null ? NotFound() : Ok(team);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(TeamCreateDto dto)
        {
            var userId = GetUserId();
            var team = await _teamsService.Create(userId, dto);
            return Created(GetBasePath(team.Id), team);
        }

        [HttpGet("firstdriver/{teamId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetFirstDriver(Guid teamId)
        {
            var component = await _teamsService.GetFirstDriver(teamId);
            return component == null ? NotFound() : Ok(component);
        }
        [HttpGet("seconddriver/{teamId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetSecondDriver(Guid teamId)
        {
            var component = await _teamsService.GetSecondDriver(teamId);
            return component == null ? NotFound() : Ok(component);
        }
        [HttpGet("engine/{teamId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetEngine(Guid teamId)
        {
            var component = await _teamsService.GetEngine(teamId);
            return component == null ? NotFound() : Ok(component);
        }
        [HttpGet("chassis/{teamId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetChassis(Guid teamId)
        {
            var component = await _teamsService.GetChassis(teamId);
            return component == null ? NotFound() : Ok(component);
        }


        [HttpGet("firstdriver/{driverId:guid}/buy")]
        [Authorize]
        public async Task<IActionResult> BuyFirstDriver(Guid driverId)
        {
            var userId = GetUserId();
            var componentInfo = await _teamsService.BuyFirstDriver(userId, driverId);
            return componentInfo == null ? BadRequest() : Ok(componentInfo);
        }
        [HttpGet("seconddriver/{driverId:guid}/buy")]
        [Authorize]
        public async Task<IActionResult> BuySecondDriver(Guid driverId)
        {
            var userId = GetUserId();
            var componentInfo = await _teamsService.BuySecondDriver(userId, driverId);
            return componentInfo == null ? BadRequest() : Ok(componentInfo);
        }
        [HttpGet("engine/{engineId:guid}/buy")]
        [Authorize]
        public async Task<IActionResult> BuyEngine(Guid engineId)
        {
            var userId = GetUserId();
            var componentInfo = await _teamsService.BuyEngine(userId, engineId);
            return componentInfo == null ? BadRequest() : Ok(componentInfo);
        }
        [HttpGet("chassis/{chassisId:guid}/buy")]
        [Authorize]
        public async Task<IActionResult> BuyChassis(Guid chassisId)
        {
            var userId = GetUserId();
            var componentInfo = await _teamsService.BuyChassis(userId, chassisId);
            return componentInfo == null ? BadRequest() : Ok(componentInfo);
        }


        [HttpDelete("driver/{teamDriverId:guid}")]
        [Authorize]
        public async Task<IActionResult> SellDriver(Guid teamDriverId)
        {
            var userId = GetUserId();
            var component = await _teamsService.SellDriver(userId, teamDriverId);
            return component ? Ok() : NotFound();
        }
        [HttpDelete("engine/{teamEngineId:guid}")]
        [Authorize]
        public async Task<IActionResult> SellEngine(Guid teamEngineId)
        {
            var userId = GetUserId();
            var component = await _teamsService.SellEngine(userId, teamEngineId);
            return component ? Ok() : NotFound();
        }
        [HttpDelete("chassis/{teamChassisId:guid}")]
        [Authorize]
        public async Task<IActionResult> SellChassis(Guid teamChassisId)
        {
            var userId = GetUserId();
            var component = await _teamsService.SellChassis(userId, teamChassisId);
            return component ? Ok() : NotFound();
        }


        [HttpGet("driver/{teamDriverId:guid}/confirm")]
        [Authorize]
        public async Task<IActionResult> SellDriverConfirm(Guid teamDriverId)
        {
            var userId = GetUserId();
            var component = await _teamsService.SellDriverConfirmation(userId, teamDriverId);
            return component == null ? NotFound() : Ok(component);
        }
        [HttpGet("engine/{teamEngineId:guid}/confirm")]
        [Authorize]
        public async Task<IActionResult> SellEngineConfirm(Guid teamEngineId)
        {
            var userId = GetUserId();
            var component = await _teamsService.SellEngineConfirmation(userId, teamEngineId);
            return component == null ? NotFound() : Ok(component);
        }
        [HttpGet("chassis/{teamChassisId:guid}/confirm")]
        [Authorize]
        public async Task<IActionResult> SellChassisConfirm(Guid teamChassisId)
        {
            var userId = GetUserId();
            var component = await _teamsService.SellChassisConfirmation(userId, teamChassisId);
            return component == null ? NotFound() : Ok(component);
        }

        public TeamController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

    }
}
