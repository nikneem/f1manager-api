using System;
using System.Security.Claims;
using F1Manager.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace F1Manager.Api.Base
{
    [ApiController]
    public abstract class F1ManagerApiControllerBase : ControllerBase
    {

        protected Guid? GetUserId()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdClaim?.Value, out Guid userId))
            {
                return userId;
            }

            return null;
        }

    }
}
