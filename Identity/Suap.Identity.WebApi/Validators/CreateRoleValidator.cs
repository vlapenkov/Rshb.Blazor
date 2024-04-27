using FluentValidation;
using Suap.Identity.WebApi.Dto;

namespace Suap.Identity.WebApi.Validators;

public class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^[-a-zA-Z]+$");          

    }
}
