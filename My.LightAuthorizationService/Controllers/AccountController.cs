using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using My.Auth;
using My.LightAuthorizationService.Dto;
using My.LightAuthorizationService.Entities;
using My.LightAuthorizationService.Services;


namespace My.LightAuthorizationService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService  _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginRequest login)
        {          


            var user = await _userManager.FindByEmailAsync(login.UserName);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest(new IdentResponse<string>
                {
                    Success = false,
                    Message = "Login failed"
                });
                //return BadRequest(new LoginResult { Successful = false, Error = "Login failed" });
            }

            return Ok(
                new IdentResponse<string>
                {
                    Success = true,
                    Data = await _tokenService.CreateToken(user)
                }
                //new LoginResult
                //{
                //    Successful = true,
                //    Token = await _tokenService.CreateToken(user)
                //}
            );

            

        }

       
    }
}
