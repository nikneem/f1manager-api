using System;
using System.Threading.Tasks;
using F1Manager.Admin.Engines.Abstractions;
using F1Manager.Admin.Engines.DataTransferObjects;
using F1Manager.Api.Base;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnginesController : F1ManagerApiControllerBase
    {
        private readonly IEnginesService _enginesService;

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var active = await _enginesService.GetActive();
            return Ok(active);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] EnginesListFilterDto filter)
        {
            var engines = await _enginesService.List(filter);
            return Ok(engines);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Single(Guid id)
        {
            var engines = await _enginesService.Get(id);
            return Ok(engines);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EngineDetailsDto dto)
        {
            var createdEngine = await _enginesService.Create(dto);
            return createdEngine != null ? Ok(createdEngine) : BadRequest();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, EngineDetailsDto dto)
        {
            var updatedEngine = await _enginesService.Update(id, dto);
            return updatedEngine != null ? Ok(updatedEngine) : BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedEngine = await _enginesService.Delete(id);
            return deletedEngine ? Ok() : BadRequest();
        }

        [HttpDelete("{id:guid}/undelete")]
        public async Task<IActionResult> Undelete(Guid id)
        {
            var undeletedEngine = await _enginesService.Undelete(id);
            return undeletedEngine ? Ok() : BadRequest();
        }

        public EnginesController(IEnginesService enginesService)
        {
            _enginesService = enginesService;
        }
    }
}
