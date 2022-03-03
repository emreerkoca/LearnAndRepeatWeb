using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LearnAndRepeatWeb.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("health-check")]
        public IActionResult Get()
        {
            return StatusCode((int)HttpStatusCode.OK, Environment.MachineName);
        }
    }
}
