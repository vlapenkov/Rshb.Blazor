using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;



public class ValidationFieldError
{
    
    [Required]
    public string Name { get; set; }

    
    [Required]
    public string[] Messages { get; set; }

    
}