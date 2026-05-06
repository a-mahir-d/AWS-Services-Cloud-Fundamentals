using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var client = new AmazonSecretsManagerClient();

var request = new GetSecretValueRequest
{
    SecretId = "apiKey"
};
var response = await client.GetSecretValueAsync(request);
Console.WriteLine(response.SecretString);

var listSecretVersionRequest = new ListSecretVersionIdsRequest
{
    SecretId = "apiKey",
    IncludeDeprecated = true
};
var versionResponse = await client.ListSecretVersionIdsAsync(listSecretVersionRequest);
request = new GetSecretValueRequest
{
    SecretId = "apiKey",
    VersionId = versionResponse.Versions[0].VersionId
};
response = await client.GetSecretValueAsync(request);
Console.WriteLine(response.SecretString);