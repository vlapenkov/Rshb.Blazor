using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suap.Triast.Dto;
using System;
using Suap.Common.Exceptions;
using System.Text.Json.Nodes;
using Suap.Triast.WebApi.Extensions;
using Suap.Common.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Suap.Triast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        
        [HttpGet]
        [Authorize]
        //[Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "offline_access1")]
        //[Authorize(AuthenticationSchemes = AuthSchemas.Jwt, Policy = "AdminPolicy")]        


        public IEnumerable<WeatherForecast> Get()
        {

           

            WeatherForecast wf = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
            _logger.LogInformation("Сущность  с Id={Id} создана. {@wf}", Guid.NewGuid(), wf);

                           

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]        
        public IActionResult Post([FromBody] WeatherForecast weatherForecast)
        {
            return Ok(weatherForecast);
        }
    }
}
