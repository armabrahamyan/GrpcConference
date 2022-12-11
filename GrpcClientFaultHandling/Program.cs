using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using static GrpcService.Greeter;


var defaultMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable }
    }
};


var channel = GrpcChannel.ForAddress("https://localhost:7247", new GrpcChannelOptions
{
    ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
});
var client = new GreeterClient(channel);
    
var reply = await client.SayHelloAsync(new GrpcService.HelloRequest
{
    Name = "Purlin",
});

Console.WriteLine($"Reply from server {reply.Message}");