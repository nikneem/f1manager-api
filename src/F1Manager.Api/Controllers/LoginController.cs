using System.Threading.Tasks;
using F1Manager.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var loginAttempt = await _loginService.RequestLogin();
            return Ok(loginAttempt);
        }

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

    }
}
