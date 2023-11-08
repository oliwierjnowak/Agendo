using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Agendo.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendo.Server.Controllers
{
    //add authorization to the controllers
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDomainService _domainRepository;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDomainService domainRepository)
        {
            _logger = logger;
            _domainRepository = domainRepository;
        }

        [HttpGet]
        [Authorize(Roles = "719")]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}