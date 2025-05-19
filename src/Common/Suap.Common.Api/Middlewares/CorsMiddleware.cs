﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suap.Common.Api.Middlewares;
public class CorsMiddleware
{
    private readonly RequestDelegate _next;

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("Origin", out var originValue))
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", originValue);

            if (httpContext.Request.Method == "OPTIONS")
            {
                httpContext.Response.StatusCode = 204;
                return httpContext.Response.WriteAsync(string.Empty);
            }
        }

        return _next(httpContext);
    }
}

public static class CorsMiddlewareExtensions
{
    public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsMiddleware>();
    }
}
