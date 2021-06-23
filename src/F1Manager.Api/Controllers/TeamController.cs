using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Teams.Abstractions;
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
            var team = await _teamsService.GetByUserId(userId.GetValueOrDefault());
            return team == null ? NotFound() : Ok(team);
        }

        public TeamController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

    }
}
