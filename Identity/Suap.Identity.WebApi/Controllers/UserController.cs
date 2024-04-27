using Microsoft.AspNetCore.Mvc;
using Suap.Identity.Logic.Dto;
using Suap.Identity.Logic.Interfaces;


namespace Suap.IdentityService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
            await _userService.Create(request);

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

            await _userService.AddRoleToUser(request);

            return Ok();



        }
    }
}
