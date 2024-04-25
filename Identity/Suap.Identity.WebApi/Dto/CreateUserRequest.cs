namespace Suap.Identity.WebApi.Dto;

public record CreateUserRequest(string UserName, string Email, string Password);
