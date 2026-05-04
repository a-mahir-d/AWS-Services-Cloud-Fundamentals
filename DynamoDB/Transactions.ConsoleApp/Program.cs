using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Transactions.ConsoleApp;

var dynamoDbClient = new AmazonDynamoDBClient();

var shoppingCart = new ShoppingCart
{
    Pk = "1",
    Sk = "1",
    ProductName = "Domates",
};

var order = new Order
{
    Pk = "1",
    Sk = "1",
    ProductName = "Domates",
};

var shoppingCartAsJson = JsonSerializer.Serialize(shoppingCart);
var orderAsJson = JsonSerializer.Serialize(order);

var shoppingCartAttributeMap = Document.FromJson(shoppingCartAsJson).ToAttributeMap();
var orderAttributeMap = Document.FromJson(orderAsJson).ToAttributeMap();

var transactRequest = new TransactWriteItemsRequest
{
    TransactItems =
    [
        new TransactWriteItem
        {
            Put = new Put
            {
                TableName = "shopping-carts",
                Item = shoppingCartAttributeMap
            }
        },

        new TransactWriteItem
        {
            Put = new Put
            {
                TableName = "orders",
                Item = orderAttributeMap
            }
        }
    ]
};

var response = await dynamoDbClient.TransactWriteItemsAsync(transactRequest);