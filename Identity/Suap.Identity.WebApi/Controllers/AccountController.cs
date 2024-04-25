using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Suap.Common.Exceptions;
using Suap.Identity.Contracts;
using Suap.Identity.Domain;
using Suap.IdentityService.Dto;
using Suap.IdentityService.Services;



namespace Suap.IdentityService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>        

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            string errorAuthenticationMessage = "Неуспешная попытка аутентификации";

            var user = await _userManager.FindByEmailAsync(login.UserName);

            if (user == null)
            {
                throw new AppException(errorAuthenticationMessage);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded)
            {
                throw new AppException(errorAuthenticationMessage);
            }

            var token = await _tokenService.CreateToken(user);

            return Ok(TokenResponse.FromSuccess(token));


        }


    }
}
