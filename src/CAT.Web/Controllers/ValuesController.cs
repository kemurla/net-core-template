using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CAT.Web.Controllers
{
    /// <summary>
    /// A simple dummy Controller that was created in order to test the authentication.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
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

        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), (int)HttpStatusCode.OK)]
        [HttpGet("weather")]
        public IEnumerable<WeatherForecast> GetWeather()
        {
            _logger.LogInformation("Getting the weather.");

            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}