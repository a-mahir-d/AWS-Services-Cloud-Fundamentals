using System.Text.Json.Serialization;

namespace Transactions.ConsoleApp;

public class ShoppingCart
{
    [JsonPropertyName("pk")]
    public required string Pk { get; set; }
    
    [JsonPropertyName("sk")]
    public required string Sk { get; set; }
    
    public string ProductName { get; set; } = string.Empty;
}