using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using GrpcService;
using Grpc.Net.Client.Configuration;

var factory = new StaticResolverFactory(addr => new[]
{
    new BalancerAddress("localhost", 7247),
    new BalancerAddress("localhost", 7248)
});

var services = new ServiceCollection();
services.AddSingleton<ResolverFactory>(factory);

var channel = GrpcChannel.ForAddress(
    "static:///localhost",
    new GrpcChannelOptions
    {
        Credentials = ChannelCredentials.SecureSsl,
        ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } },
        ServiceProvider = services.BuildServiceProvider()
    });
var client = new Greeter.GreeterClient(channel);


for (int i = 1; i < 10; i++)
{
    var reply = await client.SayHelloAsync(new HelloRequest
    {
        Name = "Purlin co.",
    });

    Console.WriteLine(reply.Message + $"request N: {i}");
}
    
