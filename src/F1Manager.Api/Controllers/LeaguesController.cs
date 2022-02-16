using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Leagues.Abstractions;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
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

        public LeaguesController(ILeaguesService leaguesService, ITeamsService teamsService)
        {
            _leaguesService = leaguesService;
            _teamsService = teamsService;
        }

    }
}
