using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiWithMediatR.MediatR;

namespace WebApiWithMediatR.Controllers
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
    private readonly IMediator mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
      _logger = logger;
      this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<string>> TestMediatR()
    {
      await this.mediator.Send(new CreateUserCommand { Name = "lqclass.com" });
      return Ok("OK");
    }
  }
}
