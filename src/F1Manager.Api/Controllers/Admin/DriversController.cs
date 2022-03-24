using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using F1Manager.Admin.Drivers.Abstractions;
using F1Manager.Admin.Drivers.DataTransferObjects;
using F1Manager.Api.Base;
using Microsoft.AspNetCore.Authorization;

namespace F1Manager.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DriversController : F1ManagerApiControllerBase
    {
        private readonly IDriversService _driversService;

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var activeDrivers = await _driversService.GetActive();
            return Ok(activeDrivers);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] DriversListFilterDto filter)
        {
            var drivers = await _driversService.List(filter);
            return Ok(drivers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Single(Guid id)
        {
            var driver = await _driversService.Get(id);
            return Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DriverDetailsDto dto)
        {
            var createdDriver = await _driversService.Create(dto);
            return createdDriver != null ? Ok(createdDriver) : BadRequest();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, DriverDetailsDto dto)
        {
            var updatedDriver = await _driversService.Update(id, dto);
            return updatedDriver != null ? Ok(updatedDriver) : BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedDriver = await _driversService.Delete(id);
            return deletedDriver ? Ok() : BadRequest();
        }

        [HttpDelete("{id:guid}/undelete")]
        public async Task<IActionResult> Undelete(Guid id)
        {
            var undeletedDriver = await _driversService.Undelete(id);
            return undeletedDriver  ? Ok() : BadRequest();
        }

        public DriversController(IDriversService driversService)
        {
            _driversService = driversService;
        }
    }
}
