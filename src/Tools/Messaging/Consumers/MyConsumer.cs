using MassTransit;
using Serilog;
using Tools.Messaging.Messages;

namespace Tools.Messaging.Consumers;

internal class MyConsumer : IConsumer<HelloMessage>
{
    public Task Consume(ConsumeContext<HelloMessage> context)
    {
        Log.Information($"Hello {context.Message.Value}");
        return Task.CompletedTask;
    }
}
