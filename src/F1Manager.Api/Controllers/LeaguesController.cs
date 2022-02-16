using System;
using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Leagues.Abstractions;
using F1Manager.Leagues.DataTransferObjects;
using F1Manager.Teams.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : F1ManagerApiControllerBase
    {
        private readonly ILeaguesService _leaguesService;
        private readonly ITeamsService _teamsService;

        [HttpGet("mine")]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leaguesList = await _leaguesService.List(team.Id);
                return Ok(leaguesList);
            }

            return NotFound();
        }
        [HttpGet("{leagueId:guid}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid leagueId)
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leagueDetails = await _leaguesService.Get(leagueId, team.Id);
                return Ok(leagueDetails);
            }

            return NotFound();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateLeagueDto dto)
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leagueDetails = await _leaguesService.Create(dto, userId, team.Id);
                return Ok(leagueDetails);
            }

            return NotFound();
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(LeagueDto dto)
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leagueDetails = await _leaguesService.Update(dto, userId, team.Id);
                return Ok(leagueDetails);
            }

            return NotFound();
        }

        [HttpPost("validate")]
        [Authorize]
        public async Task<IActionResult> Validate(CreateLeagueDto dto)
        {
            var leagueDetails = await _leaguesService.Validate(dto);
            return leagueDetails ? Ok() : BadRequest();
        }


        [HttpGet("{leagueId:guid}/invite/{teamId:guid}")]
        [Authorize]
        public async Task<IActionResult> CreateInvite(Guid leagueId, Guid teamId)
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leagueDetails = await _leaguesService.Invite(leagueId, userId, team.Id, teamId);
                return Ok(leagueDetails);
            }

            return NotFound();
        }
        [HttpGet("{leagueId:guid}/invite/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptInvite(Guid leagueId)
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leagueDetails = await _leaguesService.AcceptInvitation(leagueId, team.Id);
                return Ok(leagueDetails);
            }

            return NotFound();
        }
        [HttpGet("{leagueId:guid}/invite/decline")]
        [Authorize]
        public async Task<IActionResult> DeclineInvite(Guid leagueId)
        {
            var userId = GetUserId();
            var team = await _teamsService.GetByUserId(userId);
            if (team != null)
            {
                var leagueDetails = await _leaguesService.DeclineInvitation(leagueId, team.Id);
                return Ok(leagueDetails);
            }

            return NotFound();
        }

        public LeaguesController(ILeaguesService leaguesService, ITeamsService teamsService)
        {
            _leaguesService = leaguesService;
            _teamsService = teamsService;
        }

    }
}
