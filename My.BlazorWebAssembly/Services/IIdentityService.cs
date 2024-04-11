using My.BlazorWebAssembly.Dto;

namespace My.BlazorWebAssembly.Services
{
    public interface IIdentityService
    {
        Task<IdentResponse<string>> Login(UserLogin userLogin);
    }
}