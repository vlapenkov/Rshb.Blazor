using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Suap.Common.Exceptions;
using Suap.Identity.Domain;
using Suap.Identity.WebApi.Dto;


namespace Suap.IdentityService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;        

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        /// <summary>
        /// Создание пользователя
        /// </summary>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            AppUser user;

            user = await _userManager.FindByNameAsync(request.UserName);

            if (user != null)
            {
                throw new AppException($"Пользователь {request.UserName} уже существует");
            }

            user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new AppException($"Пользователь {request.Email} уже существует");
            }

            var result = await _userManager.CreateAsync(new AppUser { UserName = request.UserName, Email = request.Email }, request.Password);

            if (!result.Succeeded)
            {
                throw new AppException(result.Errors.First().Description);
            }

            return Ok();



        }

        /// <summary>
        /// Добавление пользователя к роли
        /// </summary>   
        [HttpPost("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddUserToRoleRequest request)
        {                        

            AppUser user = await _userManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                throw new AppException($"Пользователя {request.UserName} не существует");
            }


            AppRole? role = await _roleManager.FindByNameAsync(request.RoleName);
            
            if (role is null)
                throw new AppException($"Роли {request.RoleName} не существует");

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
                        

            if (!result.Succeeded)
            {
                throw new AppException(result.Errors.First().Description);
            }

            return Ok();



        }
    }
}
