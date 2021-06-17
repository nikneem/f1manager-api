using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Users.Abstractions;
using F1Manager.Users.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : F1ManagerApiControllerBase
    {
        private readonly IUsersService _service;

        [HttpPost]
        public async Task<IActionResult> Post(UserRegistrationDto dto)
        {
            var response = await _service.Register(dto);
            return Ok(response);
        }



        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var userId = GetUserId();
            return Ok();
        }

        public UsersController(IUsersService service)
        {
            _service = service;
        }

    }
}
