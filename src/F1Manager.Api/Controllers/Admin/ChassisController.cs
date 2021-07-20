using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using F1Manager.Admin.Chassises.Abstractions;
using F1Manager.Admin.Chassises.DataTransferObjects;
using F1Manager.Api.Base;

namespace F1Manager.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChassisController : F1ManagerApiControllerBase
    {
        private readonly IChassisesService _chassisesService;

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var active = await _chassisesService.GetActive();
            return Ok(active);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ChassisListFilterDto filter)
        {
            var engines = await _chassisesService.List(filter);
            return Ok(engines);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Single(Guid id)
        {
            var engines = await _chassisesService.Get(id);
            return Ok(engines);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChassisDetailsDto dto)
        {
            var createdEngine = await _chassisesService.Create(dto);
            return createdEngine != null ? Ok(createdEngine) : BadRequest();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ChassisDetailsDto dto)
        {
            var updatedEngine = await _chassisesService.Update(id, dto);
            return updatedEngine != null ? Ok(updatedEngine) : BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedEngine = await _chassisesService.Delete(id);
            return deletedEngine ? Ok() : BadRequest();
        }

        [HttpDelete("{id:guid}/undelete")]
        public async Task<IActionResult> Undelete(Guid id)
        {
            var undeletedEngine = await _chassisesService.Undelete(id);
            return undeletedEngine ? Ok() : BadRequest();
        }

        public ChassisController(IChassisesService chassisesService)
        {
            _chassisesService = chassisesService;
        }
    }
}
