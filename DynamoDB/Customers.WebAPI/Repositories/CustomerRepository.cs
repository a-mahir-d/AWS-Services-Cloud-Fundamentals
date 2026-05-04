using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.WebAPI.Models;
using Customers.WebAPI.Models.Dtos;

namespace Customers.WebAPI.Repositories;

public sealed class CustomerRepository(IAmazonDynamoDB dynamoDb)
{
    private const string TableName = "customers";

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var scanRequest = new ScanRequest
        {
            TableName = TableName
        };
        
        var response = await dynamoDb.ScanAsync(scanRequest);
        return response.Items.Select(s =>
        {
            var json = Document.FromAttributeMap(s).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(json);
        })!;
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        var queryRequest = new QueryRequest
        {
            TableName = TableName,
            IndexName = "email-id-index",
            KeyConditionExpression = "Email = :v_Email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":v_Email", new AttributeValue { S = email } }
            }
        };
        
        var response = await dynamoDb.QueryAsync(queryRequest);
        if (response.Items.Count == 0) return null;
        var itemAsDocument = Document.FromAttributeMap(response.Items[0]);
        var json = itemAsDocument.ToJson();
        return JsonSerializer.Deserialize<CustomerDto>(json);
    }
    
    public async Task<bool> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer{ Name = dto.Name, Email = dto.Email };
        
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttribute = Document.FromJson(customerAsJson).ToAttributeMap();
        
        var createItemRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = customerAsAttribute,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)",
        };
        var response = await dynamoDb.PutItemAsync(createItemRequest);
        
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
    
    public async Task<bool> UpdateAsync(UpdateCustomerDto dto, DateTime requestStarted)
    {
        var customer = new Customer{ Id = dto.Id, Name = dto.Name, Email = dto.Email, UpdatedAt = DateTime.UtcNow };
        
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttribute = Document.FromJson(customerAsJson).ToAttributeMap();
        
        var updateItemRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = customerAsAttribute,
            ConditionExpression = "UpdatedAt < :requestStarted",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":requestStarted", new AttributeValue{S = requestStarted.ToString("O")} },
            }
        };
        var response = await dynamoDb.PutItemAsync(updateItemRequest);
        
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };
        
        var response = await dynamoDb.DeleteItemAsync(deleteItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}