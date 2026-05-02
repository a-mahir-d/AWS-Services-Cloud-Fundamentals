using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQSWebAPI.Consumer.Messaging;

public sealed class ReceiveMessage
{
    public async Task ReceiveMessageAsync<T>(CancellationToken cancellationToken = default)
    {
        var region = Amazon.RegionEndpoint.EUNorth1;
        var sqsClient = new AmazonSQSClient(region);
        var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers", cancellationToken);

        var receivedMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageSystemAttributeNames = ["All"],
            MessageAttributeNames = ["All"]
        };

        while (!cancellationToken.IsCancellationRequested)
        {
            var response = await sqsClient.ReceiveMessageAsync(receivedMessageRequest, cancellationToken);
            if (response?.Messages?.Count > 0)
            {
                foreach (var message in response.Messages)
                {
                    try
                    {
                        Console.WriteLine($"Message Id: {message.MessageId}, Body: {JsonSerializer.Deserialize<T>(message.Body)}, Attribute Count: {message.Attributes.Count}");
                        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            await Task.Delay(1000, cancellationToken);
        }
    }
}