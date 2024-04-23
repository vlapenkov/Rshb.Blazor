using FluentValidation;
using Suap.Triast.Dto;


namespace Suap.Triast.Validators;

public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
{
    public WeatherForecastValidator()
    {
        RuleFor(p => p.TemperatureC)
            .GreaterThan(10)
            .LessThan(50)
            .NotEmpty();

        RuleFor(p => p.Date)
            .NotEmpty()
            .Must(p => p > DateOnly.FromDateTime(DateTime.UtcNow));

    }
}