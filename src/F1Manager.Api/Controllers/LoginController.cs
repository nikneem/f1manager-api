using System.Threading.Tasks;
using F1Manager.Api.Base;
using F1Manager.Users.Abstractions;
using F1Manager.Users.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : F1ManagerApiControllerBase
    {
        private readonly ILoginService _loginService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var loginAttempt = await _loginService.RequestLogin();
            return Ok(loginAttempt);
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequestDto dto)
        {
            var response = await _loginService.Login(dto);
            return Ok(response);
        }
        [HttpGet("recover")]
        [Authorize]
        public async Task<IActionResult> Recover()
        {
            var userId = GetUserId().GetValueOrDefault();
            var response = await _loginService.Recover(userId);
            return Ok(response);
        }

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

    }
}
