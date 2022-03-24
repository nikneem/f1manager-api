using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using F1Manager.Admin.Chassises.Abstractions;
using F1Manager.Admin.Chassises.DataTransferObjects;
using F1Manager.Api.Base;
using Microsoft.AspNetCore.Authorization;

namespace F1Manager.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var chassis = await _chassisesService.List(filter);
            return Ok(chassis);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Single(Guid id)
        {
            var chassis = await _chassisesService.Get(id);
            return Ok(chassis);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChassisDetailsDto dto)
        {
            var createdChassis = await _chassisesService.Create(dto);
            return createdChassis != null ? Ok(createdChassis) : BadRequest();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ChassisDetailsDto dto)
        {
            var updatedChassis = await _chassisesService.Update(id, dto);
            return updatedChassis != null ? Ok(updatedChassis) : BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedChassis = await _chassisesService.Delete(id);
            return deletedChassis ? Ok() : BadRequest();
        }

        [HttpDelete("{id:guid}/undelete")]
        public async Task<IActionResult> Undelete(Guid id)
        {
            var undeletedChassis = await _chassisesService.Undelete(id);
            return undeletedChassis ? Ok() : BadRequest();
        }

        public ChassisController(IChassisesService chassisesService)
        {
            _chassisesService = chassisesService;
        }
    }
}
