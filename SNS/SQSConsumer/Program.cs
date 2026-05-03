using Amazon.SQS;
using Amazon.SQS.Model;

var sqsClient = new AmazonSQSClient();

var queueName = args.Length == 1 ? args[0] : "customers";
var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueName);

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageSystemAttributeNames = ["All"]
};

var cts = new CancellationTokenSource();

while (!cts.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
    if (response?.Messages?.Count > 0)
    {
        foreach (var message in response.Messages)
        {
            try
            {
                Console.WriteLine($"Message Id: {message.MessageId}, Body: {message.Body}, Attribute Count: {message.Attributes.Count}");
                await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    await Task.Delay(1000, cts.Token);
}