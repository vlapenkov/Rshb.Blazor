using FluentValidation;
using Suap.Identity.WebApi.Dto;

namespace Suap.Identity.WebApi.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(p => p.UserName)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^[-a-zA-Z]+$");

        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty()
            .MinimumLength(6);

    }
}
