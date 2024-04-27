//using FluentValidation;

using System.ComponentModel;


public class ValidationError : BaseError
{
    const char separator = ',';
   
    public List<ValidationFieldError> Fields { get; set; }

    public override string ToString()
    {
        string fieldsData = String.Empty;

        if (Fields != null && Fields.Any())
        {

            fieldsData = String.Join(separator, Fields.Select(x => x.Name + String.Join(separator, x.Messages)));
                    
        }

        return $"{Error} {fieldsData}";

    }
}