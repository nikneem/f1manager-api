using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using F1Manager.Abstractions.Users.DataTransferObjects;
using F1Manager.Abstractions.Users.Services;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly IUsersService _service;

        [HttpPost]
        public async Task<IActionResult> Post(UserRegistrationDto dto)
        {
            var response = await _service.Register(dto);
            return Ok(response);
        }


        public Users(IUsersService service)
        {
            _service = service;
        }

    }
}
