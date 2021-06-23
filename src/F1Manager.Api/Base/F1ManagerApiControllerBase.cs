using System;
using System.Net;
using System.Security.Claims;
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
        protected IPAddress GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For") &&
                IPAddress.TryParse(Request.Headers["X-Forwarded-For"], out IPAddress address))
                return address;

            return HttpContext.Connection.RemoteIpAddress;
        }

        protected string GetBasePath(Guid? includeId)
        {
            return $"{Request.Scheme}://{Request.Host}{Request.PathBase}{includeId}";
        }
    }
}
