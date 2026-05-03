using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace SNSWebAPI.Publisher.Messaging;

public class SendMessage
{
    public async Task<PublishResponse> SendMessageAsync<T>(T message, string value, CancellationToken cancellationToken = default)
    {
        var snsClient = new AmazonSimpleNotificationServiceClient();

        var topicArnResponse = await snsClient.FindTopicAsync("customers");

        var publishRequest = new PublishRequest()
        {
            TopicArn = topicArnResponse.TopicArn,
            Message = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = value
                    }
                }
            }
        };

        var response = await snsClient.PublishAsync(publishRequest, cancellationToken);
        return response;
    }
}