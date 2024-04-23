using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public abstract class BaseError 
{
    [DisplayName("Текст ошибки")]
    [Required]
    public string Error { get; init; }
}