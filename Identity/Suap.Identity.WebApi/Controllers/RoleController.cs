using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Suap.Common.Exceptions;
using Suap.Identity.Domain;
using Suap.Identity.WebApi.Dto;


namespace Suap.IdentityService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Создание роли
        /// </summary>      
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {

            var role = await _roleManager.FindByNameAsync(request.Name);

            if (role != null)
            {
                throw new AppException($"Роль {request.Name} уже существует");
            }

            var result = await _roleManager.CreateAsync(new AppRole { Name = request.Name });

            if (!result.Succeeded)
            {
                throw new AppException(result.Errors.First().Description);
            }

            return Ok();

        }


    }
}
