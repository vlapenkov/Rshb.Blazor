using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Suap.Common.Api.Middlewares;
using System.Text.Json.Serialization;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Linq.Expressions;
using System.Text.Json;
using System.ComponentModel;

namespace Suap.Common.Api;
public static class HostExtensions
{
    public static void RunApi(this WebApplicationBuilder builder,
        Action<IHostBuilder, IConfiguration, IServiceCollection> configureBuilder,
        Action<WebApplication>? configureApp = null)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        builder.Host.UseLogging();


        //builder.Services.AddCors();
        builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
           builder =>
           {
               builder.AllowAnyHeader()
                      .AllowAnyMethod()
                      .SetIsOriginAllowed((host) => true)
                      .AllowCredentials();
           }));

        builder.Services.AddHealthChecks();

        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = c => new BadRequestObjectResult(new ValidationError(c.ModelState));
            })
            .AddJsonOptions(options =>
            {
                //  options.JsonSerializerOptions.Converters.Add(new JsonNameEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

        if (!builder.Environment.IsProduction())
        {

            builder.Services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);

                //c.SchemaFilter<DisplayNameSchemaFilter>();
            });

            builder.Services.AddFluentValidationRulesToSwagger();
        }


        ValidatorOptions.Global.PropertyNameResolver =
     (Type type, MemberInfo memberInfo, LambdaExpression expression) =>
         JsonNamingPolicy.CamelCase.ConvertName(memberInfo?.Name ?? string.Empty);
        ValidatorOptions.Global.DisplayNameResolver =
            (Type type, MemberInfo memberInfo, LambdaExpression expression) =>
            {
                var a = memberInfo?.GetCustomAttribute<DisplayNameAttribute>();
                return a != null ? a.DisplayName : memberInfo?.Name;
            };

        builder.Services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
        builder.Services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true);

        configureBuilder(builder.Host, builder.Configuration, builder.Services);

        WebApplication app = builder.Build();

        app.UseCors("CorsPolicy");

        //app.UseCorsMiddleware();
        //app.UseCors(builder => 
        //builder.WithOrigins("http://192.168.0.108:3000")
        //builder.AllowAnyOrigin()
        //                  .AllowAnyHeader()
        //                  .AllowAnyMethod());
        //app.UseCors(x => x
        // .AllowAnyOrigin()
        // .AllowAnyMethod()
        // .AllowAnyHeader()
        // //.SetIsOriginAllowed(origin => true) // allow any origin
        // .AllowCredentials()); // allow credentials

        app.MapHealthChecks("/healthz");


        if (!builder.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseRouting();

        if (configureApp != null)
            configureApp(app);

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}
