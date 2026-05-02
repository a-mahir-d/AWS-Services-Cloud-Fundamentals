using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

var accessKey = "";
var secretKey = "";
var region = Amazon.RegionEndpoint.EUNorth1;

// var sqsClient = new AmazonSQSClient(accessKey, secretKey, region);  // Console'daki kullanıcı verilerine erişim sağlayabiliyor
var sqsClient = new AmazonSQSClient(region);

var customer = new
{
    FirstName = "John",
    LastName = "Doe",
    Age = 35
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");
var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = "Customer"
            }
        }
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine("Message sent");