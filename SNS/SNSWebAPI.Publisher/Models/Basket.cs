namespace SNSWebAPI.Publisher.Models;

public class Basket
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    

    public static List<Basket> GetAll()
    {
        return
        [
            new Basket()
            {
                ProductName = "Apple",
                Quantity = 1,
                Price = 10
            },

            new Basket()
            {
                ProductName = "Orange",
                Quantity = 2,
                Price = 15
            },

            new Basket()
            {
                ProductName = "Banana",
                Quantity = 3,
                Price = 20
            }
        ];
    }
}