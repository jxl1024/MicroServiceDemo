using Microsoft.AspNetCore.Mvc;
using System;

namespace MicroService.AggregateService.Controllers
{
    [Route("HealthCheck")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetHealthCheck()
        {
            Console.WriteLine($"进行心跳检测:{DateTime.Now}");
            return Ok("连接正常");
        }
    }
}
