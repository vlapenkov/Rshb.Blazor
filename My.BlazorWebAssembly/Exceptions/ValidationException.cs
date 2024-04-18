namespace My.BlazorWebAssembly.Exceptions;

public class ValidationException : Exception
{
    public List<ValidationFieldError>? Fields { get; set; }
    public ValidationException(string message, List<ValidationFieldError> fields) : base(message)
    {
        Fields = fields;
    }

    
}
