using Amazon.S3;
using Amazon.S3.Model;

var inputStream = new FileStream("./dummy.json", FileMode.Open, FileAccess.Read);

var s3Client = new AmazonS3Client();

var putObjectRequest = new PutObjectRequest()
{
    BucketName = "amd.file.system",
    Key = "files/dictionary.json",
    ContentType = "application/json",
    InputStream = inputStream
};

await s3Client.PutObjectAsync(putObjectRequest);