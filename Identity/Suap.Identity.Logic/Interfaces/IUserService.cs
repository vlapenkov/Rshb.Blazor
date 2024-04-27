using Suap.Identity.Logic.Dto;

namespace Suap.Identity.Logic.Interfaces;
public interface IUserService
{
    Task AddRoleToUser(AddUserToRoleRequest request);
    Task Create(CreateUserRequest request);
}