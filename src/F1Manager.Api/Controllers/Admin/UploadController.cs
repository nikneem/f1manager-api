using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using F1Manager.Admin.Abstractions;
using F1Manager.Admin.DataTransferObjects;
using F1Manager.Api.Base;
using Microsoft.AspNetCore.Authorization;

namespace F1Manager.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : F1ManagerApiControllerBase
    {
        private readonly IUploadService _uploadService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SasTokenRequestDto dto)
        {
            var sasToken = await _uploadService.GetSasToken(dto);
            return Ok(sasToken);
        }

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
    }
}
