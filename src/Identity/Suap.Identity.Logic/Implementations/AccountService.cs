using Microsoft.AspNetCore.Identity;
using Suap.Common.Exceptions;
using Suap.Identity.Contracts;
using Suap.Identity.Domain;
using Suap.Identity.Logic.Dto;
using Suap.Identity.Logic.Interfaces;
using Suap.IdentityService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suap.Identity.Logic.Implementations;
public class AccountService : IAccountService
{

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<string> Login(LoginRequest model)
    {
        string errorAuthenticationMessage = "Неуспешная попытка аутентификации";

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            throw new AppException(errorAuthenticationMessage);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
        {
            throw new AppException(errorAuthenticationMessage);
        }

        var token = await _tokenService.CreateToken(user);

        return token;


    }


    public async Task ChangePassword(ChangePasswordRequest model)
    {

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            throw new AppException($"Пользователь '{model.UserName}' отсутствует");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (!result.Succeeded)
        {
            throw new AppException("Нельзя поменять пароль пользователю");
        }


    }
}
