using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Tools.Messaging.Messages;

namespace Tools.Messaging;
public static class MassTransitInstaller
{
    public static IServiceCollection AddAsyncProcessing(this IServiceCollection services, IConfiguration configuration, params Assembly[] assembliesWithConsumers)
    {
        var configData = configuration.GetSection("RabbitMQSettings");
        var brokerSettings = new BrokerSettings();
        configData.Bind(brokerSettings);

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(Assembly.GetEntryAssembly());
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host: brokerSettings.Host, virtualHost: "/", h =>
                {
                    h.Username(brokerSettings.Username);
                    h.Password(brokerSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddTransient<IMessageSender, MessageSender>();
        return services;
    }
}

public class Worker(IMessageSender sender) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await sender
                .PublishMessageAsync(new HelloMessage("RabbitMQ"), stoppingToken)
                .ConfigureAwait(false);

            await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
        }
    }
}

