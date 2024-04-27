using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Suap.Common.Exceptions;
using Suap.Identity.Domain;
using Suap.Identity.Logic.Dto;
using Suap.Identity.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suap.Identity.Logic.Implementations;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Create(CreateUserRequest request)
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



    }

    public async Task AddRoleToUser(AddUserToRoleRequest request)
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


    }
}
