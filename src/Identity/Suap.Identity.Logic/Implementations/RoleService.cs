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
public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task CreateRole(CreateRoleRequest request)
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


    }

   
}
