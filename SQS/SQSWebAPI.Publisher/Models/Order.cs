namespace SQSWebAPI.Publisher.Models;

public sealed class Order
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string OrderNumber { get; set; } = Guid.CreateVersion7().ToString();
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}