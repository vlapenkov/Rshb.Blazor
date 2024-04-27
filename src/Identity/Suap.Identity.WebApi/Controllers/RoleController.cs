using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Suap.Identity.Domain;
using Suap.Identity.Logic.Dto;
using Suap.Identity.Logic.Interfaces;


namespace Suap.IdentityService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService _roleService;
      

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
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

            await _roleService.CreateRole(request);

            return Ok();

        }


    }
}
