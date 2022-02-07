using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gwenael.Application.Dtos;
using Gwenael.Application.Extensions;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Stores;
using Gwenael.Web.Extensions;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace Gwenael.Web.Areas.Api
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class RolesController : SecuredApiControllerBase
    {
        private readonly GwenaelRoleManager _roleManager;
        private readonly IMapper _mapper;

        public RolesController(GwenaelRoleManager roleManager, GwenaelUserManager userManager, IMapper mapper) : base(userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/roles")]
        [Authorize(Policy = "Permissions.Role.View")]
        public IActionResult Index(string keyword = "", int page = 1, int size = 25, string sortBy = null, bool sortDesc = false)
        {
            sortBy ??= "Name";

            var roles = _roleManager.Roles
                .Where(x => string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword))
                .Include(x => x.Claims)
                .OrderBy($"{sortBy} {(sortDesc ? "desc" : "asc")}");

            var pagedList = roles.ToPagedList(page, size);
            var pagedListDto = _mapper.Map<PagedListDto<RoleDto>>(pagedList);

            return new JsonResult(pagedListDto);
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/roles/{roleId}")]
        [Authorize(Policy = "Permissions.Role.Edit")]
        public async Task<IActionResult> Put(Guid roleId, [FromBody] RoleDto data)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound();
            }

            var usersInRoleAsync = await _userManager.GetUsersInRoleAsync(role.Name);
            if (role.Active && !data.Active && usersInRoleAsync.Any())
            {
                return ReturnBadRequestWithErrors("UsersInRole");
            }

            role.Name = data.Name;
            role.Active = data.Active;
            await _roleManager.SyncPermissionClaim(role, data.Claims);

            await _roleManager.UpdateAsync(role);

            return new JsonResult(_mapper.Map<Role, RoleDto>(role));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/roles/")]
        [Authorize(Policy = "Permissions.Role.Create")]
        public async Task<IActionResult> Post([FromBody] RoleDto data)
        {
            var role = await _roleManager.FindByNameAsync(data.Name);

            if (role != null)
            {
                return ReturnBadRequestWithErrors("DuplicateRoleName");
            }

            role = new Role(data.Name, data.Claims, data.Active);
            await _roleManager.CreateAsync(role);
            
            return new JsonResult(_mapper.Map<RoleDto>(role));
        }
    }
}