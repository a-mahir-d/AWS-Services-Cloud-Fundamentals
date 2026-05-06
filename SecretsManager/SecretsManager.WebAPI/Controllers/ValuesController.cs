using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SecretsManager.WebAPI.Options;

namespace SecretsManager.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController(IOptionsMonitor<ConnectionStringOptions> options) : Controller
{
    private readonly ConnectionStringOptions _options = options.CurrentValue;

    [HttpGet]
    public IActionResult GetConnectionString()
    {
        return Ok(_options.InMemory);
    }
}