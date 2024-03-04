using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIs.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new List<string>
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast"), Authorize(Roles = "User")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Count)]
            })
            .ToArray();
        }

        [HttpPost("add"), Authorize(Roles ="UserVip")]
        public IEnumerable<string> Add([FromBody] string value)
        {
            Summaries.Add(value);
            return Summaries;
        }

        [HttpPut("update"), Authorize(Roles = "UserVip")]
        public IEnumerable<string> Update([FromBody] UpdateModel updateModel)
        {
            Summaries = Summaries.Select(x => x == updateModel.OldValue ? updateModel.NewValue : x).ToList();
            return Summaries;
        }


        [HttpDelete("delete"), Authorize(Roles = "Admin")]
        public IEnumerable<string> Delete([FromBody] string value)
        {
            Summaries = Summaries.Where(x => !x.Contains(value)).ToList();
            return Summaries;
        }
    }
    public class UpdateModel
    {
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}