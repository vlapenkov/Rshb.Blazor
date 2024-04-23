using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Suap.Common.Exceptions;

namespace Suap.Triast.Middlewares
{

    public class ErrorHandlerMiddleware
    {
        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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

                var result = JsonSerializer.Serialize(errorModel, _jsonOptions);
                await response.WriteAsync(result);
            }
        }
    }
}
