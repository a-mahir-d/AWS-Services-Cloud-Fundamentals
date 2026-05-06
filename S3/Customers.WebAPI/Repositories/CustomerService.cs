using System.Text;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;

namespace Customers.WebAPI.Repositories;

public class CustomerService(IAmazonS3 s3)
{
    private readonly AmazonS3Client _s3Client = new AmazonS3Client();
    public async Task<PutObjectResponse> UploadJsonAsync(Guid id, string jsonContent)
    {
        using var jsonDoc = JsonDocument.Parse(jsonContent);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var prettyJson = JsonSerializer.Serialize(jsonDoc, options);
        
        var byteArray = Encoding.UTF8.GetBytes(prettyJson);
        using var inputStream = new MemoryStream(byteArray);
        
        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = "amd.file.system",
            Key = $"files/{id}.json",
            ContentType = "application/json",
            InputStream = inputStream
        };

        return await _s3Client.PutObjectAsync(putObjectRequest);
    }

    public async Task<GetObjectResponse> GetJsonAsync(Guid id)
    {
        var getObjectRequest = new GetObjectRequest
        {
            BucketName = "amd.file.system",
            Key = $"files/{id}.json"
        };

        return await _s3Client.GetObjectAsync(getObjectRequest);
    }

    public async Task<DeleteObjectResponse> DeleteJsonAsync(Guid id)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = "amd.file.system",
            Key = $"files/{id}.json"
        };

        return await _s3Client.DeleteObjectAsync(deleteObjectRequest);
    }
}