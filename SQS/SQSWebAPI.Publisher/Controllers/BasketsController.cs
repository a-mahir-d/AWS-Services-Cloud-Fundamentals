using Microsoft.AspNetCore.Mvc;
using SQSWebAPI.Publisher.Messaging;
using SQSWebAPI.Publisher.Models;

namespace SQSWebAPI.Publisher.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BasketsController(SendMessage sqs) : Controller
{
    [HttpGet]
    public async Task<IActionResult> CreateOrder()
    {
        var baskets = Basket.GetAll();
        var orders = baskets.Select(basket => new Order() { ProductName = basket.ProductName, Price = basket.Price, Quantity = basket.Quantity }).ToList();

        // DB işlemleri
        
        await sqs.SendMessageAsync(orders);
        
        return Ok(new {Message = "Order created successfully"});
    }
}