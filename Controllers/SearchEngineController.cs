using Microsoft.AspNetCore.Mvc;

namespace SearchInsightGenerator.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class SearchEngineController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SearchEngineController> _logger;

        public SearchEngineController(ILogger<SearchEngineController> logger)
        {
            _logger = logger;
        }

        /*  [HttpGet(Name = "GetWeatherForecast")]
          public IEnumerable<WeatherForecast> Get()
          {
              return Enumerable.Range(1, 5).Select(index => new WeatherForecast
              {
                  Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                  TemperatureC = Random.Shared.Next(-20, 55),
                  Summary = Summaries[Random.Shared.Next(Summaries.Length)]
              })
              .ToArray();
          }*/

        [Route("backend/GetTop10SearchResults")]
        [HttpGet]
        //[HttpGet(Name = "GetTop10SearchResults")]
        public async Task GetTop10SearchResults(string query)
        {
            await SearchEngine.GetResults(query);
           // await SearchEngineCode.GetSearchResults();
        }
    }
}
