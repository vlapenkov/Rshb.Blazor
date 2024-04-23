using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suap.Triast.Dto;
using System;
using Suap.Common.Exceptions;

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
        //[Authorize(Roles = "TestRole")]
        [Authorize(Policy = "AdminPolicy")]        
        
        
        public IEnumerable<WeatherForecast> Get()
        {

            WeatherForecast wf = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
            _logger.LogInformation("Сущность  с Id={Id} создана. {@wf}", Guid.NewGuid(), wf);

            //_logger.LogInformation("Some logging information");

            //throw new AppException("Some gets wrong");
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
