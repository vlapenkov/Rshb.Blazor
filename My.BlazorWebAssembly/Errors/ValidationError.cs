//using FluentValidation;

using System.ComponentModel;


public class ValidationError : BaseError
{
   
    public List<ValidationFieldError> Fields { get; set; }

    
}