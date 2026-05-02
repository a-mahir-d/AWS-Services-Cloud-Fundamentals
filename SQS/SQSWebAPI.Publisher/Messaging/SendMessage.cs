using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQSWebAPI.Publisher.Messaging;

public class SendMessage
{
    public async Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var region = Amazon.RegionEndpoint.EUNorth1;
        var sqsClient = new AmazonSQSClient(region);
        var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers", cancellationToken);
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };

        var response = await sqsClient.SendMessageAsync(sendMessageRequest, cancellationToken);
        return response;
    }
}