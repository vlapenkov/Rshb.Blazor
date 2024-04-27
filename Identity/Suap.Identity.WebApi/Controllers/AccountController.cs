using Microsoft.AspNetCore.Mvc;
using Suap.Identity.Contracts;
using Suap.Identity.Logic.Dto;
using Suap.Identity.Logic.Interfaces;



namespace Suap.IdentityService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>        

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {

            var token = await _accountService.Login(model);

            return Ok(TokenResponse.FromSuccess(token));


        }

        [HttpPost("changepassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {

            await _accountService.ChangePassword(model);

            return Ok();

        }



    }
}
