//using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

[DisplayName("Ошибка валидации")]
public class ValidationError : BaseError
{
    [DisplayName("Список ошибок по полям")]
    public List<ValidationFieldError> Fields { get; }

    public ValidationError(string error)
    {
        Error = error;
    }

    public ValidationError(ModelStateDictionary modelState) : this("Некорректные входные данные")
    {
        Fields = modelState
            .Where(x => x.Value?.Errors.Count > 0)
            .Select(s => new ValidationFieldError(s.Key, s.Value.Errors.Select(e => e.ErrorMessage).ToArray()))
            .ToList();
    }
}