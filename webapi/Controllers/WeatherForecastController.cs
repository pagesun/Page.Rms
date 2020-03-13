using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Page.Rms;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Rms _rms;

        public WeatherForecastController(ILogger<WeatherForecastController> logger ,Rms rms)
        {
            _logger = logger;
            _rms = rms;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            var notice = new RmsNotice
            {
                Content = new Content("测试 请忽略！"),
                ErrorCode = "AR1002",
                Level = 2,
                PointCode = "SJC52606",
                ServiceType = "Page"
            };
            var res = _rms.NoticeAsync(notice).Result;

            var restore = new RmsRestore
            {
                Content = new Content("测试 请忽略！"),
                ErrorCode = "AR1002",
                PointCode = "SJC52606",
                ResultStatus = 0
            };
            res = _rms.RestoreAsync(restore).Result;


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        private long GetTimestamp()
        {
            var ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
