using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


[JsonDerivedType(typeof(ValidationError))]
public abstract class BaseError 
{
    [DisplayName("Текст ошибки")]
    [Required]
    public string Error { get; init; }
}