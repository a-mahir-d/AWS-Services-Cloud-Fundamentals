using Amazon.SQS;
using Amazon.SQS.Model;

var region = Amazon.RegionEndpoint.EUNorth1;
var sqsClient = new AmazonSQSClient(region);

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
};

var cts = new CancellationTokenSource();

while (!cts.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
    if (response?.Messages?.Count > 0)
    {
        foreach (var message in response.Messages)
        {
            Console.WriteLine($"Message Id: {message.MessageId}, Body: {message.Body}");
            await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
        }
    }

    await Task.Delay(1000, cts.Token);
}