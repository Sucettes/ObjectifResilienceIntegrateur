using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gwenael.Domain.Authorization;
using Gwenael.Domain.Stores;

namespace Gwenael.Web.Areas.Api
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class PermissionsController : SecuredApiControllerBase
    {
        public PermissionsController(GwenaelUserManager userManager) : base(userManager)
        {
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/permissions")]
        [Authorize(Policy = "Permissions.View")]
        public IActionResult GetAll()
        {
            return new JsonResult(Permissions.Generate());
        }
    }
}