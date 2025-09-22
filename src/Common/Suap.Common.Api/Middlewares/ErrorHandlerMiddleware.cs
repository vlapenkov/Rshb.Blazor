using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Suap.Common.Errors.Exceptions;
using Suap.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Suap.Common.Api.Middlewares
{

    public class ErrorHandlerMiddleware
    {
        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull

        };

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                BaseError errorModel;
                
                switch (exception)
                {
                    case SuapValidationException e:
                        response.StatusCode = StatusCodes.Status400BadRequest;

                        ModelStateDictionary modelState = new();                        

                        modelState.AddModelError(e.Name, e.Message);                        
                        
                        errorModel = new ValidationError(modelState);
                        _logger.LogError(exception, "Бизнес ошибка:");
                        break;

                    case AppException e:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        errorModel = new ValidationError(e.Message);
                        _logger.LogError(exception, "Бизнес ошибка:");
                        break;
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        errorModel = new ServerError(exception.Message);
                        _logger.LogError(exception, "Неожиданная ошибка:");
                        break;
                }

                var result = JsonSerializer.Serialize(errorModel , _jsonOptions);
                await response.WriteAsync(result);
            }
        }
    }
}
