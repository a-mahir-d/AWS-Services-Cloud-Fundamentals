using System.Net;
using System.Text;
using Amazon.S3.Model;
using Customers.WebAPI.Models;
using Customers.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Customers.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CustomersController(CustomerService customerService) : Controller
{
    [HttpPost]
    public async Task<ActionResult> UploadJson(UploadJsonRequest request)
    {
        var result = await customerService.UploadJsonAsync(request.Id, request.JsonContent);
        return Ok(result.HttpStatusCode);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetJson([FromQuery] Guid id)
    {
        var result = await customerService.GetJsonAsync(id);

        try
        {
            using var memoryStream = new MemoryStream();
            await result.ResponseStream.CopyToAsync(memoryStream);
            var text = Encoding.Default.GetString(memoryStream.ToArray());

            return Ok(text);
        }
        catch
        {
            return NotFound();
        }
    }
    
    [HttpDelete]
    public async Task<ActionResult> DeleteJson([FromQuery] Guid id)
    {
        var result = await customerService.DeleteJsonAsync(id);
        return result.HttpStatusCode switch
        {
            HttpStatusCode.NoContent => Ok(),
            HttpStatusCode.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}