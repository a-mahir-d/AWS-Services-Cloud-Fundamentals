using Customers.WebAPI.Models.Dtos;
using Customers.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Customers.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class CustomerController(CustomerRepository customerRepository) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await customerRepository.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var result = await customerRepository.GetByEmailAsync(email);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto request)
    {
        var result = await customerRepository.CreateAsync(request);
        return result ? Ok() : BadRequest();
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(UpdateCustomerDto request)
    {
        var requestStarted = DateTime.UtcNow;
        var result = await customerRepository.UpdateAsync(request, requestStarted);
        return result ? Ok() : BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await customerRepository.DeleteByIdAsync(id);
        return result ? Ok() : BadRequest();
    }
}