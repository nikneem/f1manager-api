using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Users.Abstractions;
using F1Manager.Users.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : F1ManagerApiControllerBase
    {
        private readonly ILoginsService _loginsService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var loginAttempt = await _loginsService.RequestLogin();
            return Ok(loginAttempt);
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequestDto dto)
        {
            var response = await _loginsService.Login(dto, GetIpAddress());
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var response = await _loginsService.Refresh(dto.Token, GetIpAddress());
            return Ok(response);
        }

        public LoginController(ILoginsService loginsService)
        {
            _loginsService = loginsService;
        }

    }
}
