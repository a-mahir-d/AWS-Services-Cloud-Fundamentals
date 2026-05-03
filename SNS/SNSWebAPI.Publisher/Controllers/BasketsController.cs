using Microsoft.AspNetCore.Mvc;
using SNSWebAPI.Publisher.Messaging;
using SNSWebAPI.Publisher.Models;

namespace SNSWebAPI.Publisher.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BasketsController(SendMessage sqs) : Controller
{
    [HttpGet]
    public async Task<IActionResult> CreateOrder()
    {
        var baskets = Basket.GetAll();
        
        foreach (var basket in baskets)
        {
            var order = new Order()
            {
                ProductName = basket.ProductName,
                Price = basket.Price,
                Quantity = basket.Quantity
            };
            
            var value = order.Quantity == 1 ? "true" : "false";
            await sqs.SendMessageAsync(order, value);
        }
        
        return Ok(new {Message = "Order created successfully"});
    }
}