using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleLambda;

public class Function
{
    public void FunctionHandler(Greeting greeting, ILambdaContext context)
    {
        context.Logger.LogInformation(greeting.Message);
    }
}

public class Greeting
{
    public string Message { get; set; } = null!;
}