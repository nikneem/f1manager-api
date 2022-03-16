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
            var response = await _service.Register(dto, GetIpAddress());
            return Ok(response);
        }

        [HttpPost("resetpwd")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var ipAddress = GetIpAddress();
            var response = await _service.ResetPassword(dto, ipAddress);
            return Ok(response);
        }
        [HttpPost("resetconfirm")]
        public async Task<IActionResult> ResetConfirm(ResetPasswordVerificationDto dto)
        {
            var ipAddress = GetIpAddress();
            var response = await _service.VerifyPasswordReset(dto, ipAddress);
            return response ? Ok() : BadRequest();
        }
        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = GetUserId();
            var ipAddress = GetIpAddress();
            var response = await _service.ChangePassword(userId, dto, ipAddress);
            return response ? Ok() : BadRequest();
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
