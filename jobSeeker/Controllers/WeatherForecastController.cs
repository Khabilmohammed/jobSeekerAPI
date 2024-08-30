using jobSeeker.DataAccess.Services.IWhetherForCastService;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWheatherForcaset _wheatherForcaset;

        public WeatherForecastController(ILogger<WeatherForecastController> logger
            ,IWheatherForcaset wheatherForcaset)
        {
            _logger = logger;
            _wheatherForcaset = wheatherForcaset;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _wheatherForcaset.Get();
        }
    }
}
