using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gwenael.Application.Dtos;
using Gwenael.Application.Extensions;
using Gwenael.Application.Services;
using Gwenael.Domain.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.Areas.Api
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class UsersController : SecuredApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UsersController(IUserService userService, GwenaelUserManager userManager, IMapper mapper) : base(userManager)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/users")]
        [Authorize(Policy = "Permissions.User.View")]
        public async Task<IActionResult> GetAll(string keyword = "", int page = 1, int size = 25, string sortBy = null, bool sortDesc = false)
        {
            var users = _userService.Search(
                    sortBy,
                    sortDesc,
                    u => string.IsNullOrEmpty(keyword),
                    u => u.UserName.Contains(keyword),
                    u => u.FirstName.Contains(keyword),
                    u => u.LastName.Contains(keyword));

            var pagedList = users.ToPagedList(page, size);
            var pagedListDto = _mapper.Map<PagedListDto<UserDto>>(pagedList);
            foreach (var user in pagedList)
            {
                pagedListDto.Items.First(u => u.Id == user.Id).Roles = await _userManager.GetRolesAsync(user);
            }

            return new JsonResult(pagedListDto);
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/users/{userName}")]
        [Authorize(Policy = "Permissions.User.View")]
        public async Task<IActionResult> Get(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);

            return new JsonResult(userDto);
        }
    }
}