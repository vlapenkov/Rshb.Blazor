using Suap.Identity.Logic.Dto;

namespace Suap.Identity.Logic.Interfaces;
public interface IRoleService
{
    Task CreateRole(CreateRoleRequest request);
}