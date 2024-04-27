namespace Suap.Identity.WebApi.Dto;

public record ChangePasswordRequest (string UserName, string NewPassword);
