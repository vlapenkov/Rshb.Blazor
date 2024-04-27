using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

[DisplayName("Ошибка валидации поля")]
public class ValidationFieldError
{
    [DisplayName("Имя поля")]
    [Required]
    public string Name { get; }

    [DisplayName("Список сообщений об ошибке")]
    [Required]
    public string[] Messages { get; }

    public ValidationFieldError(string name, params string[] messages)
    {
        Name = name;
        Messages = messages;
    }
}