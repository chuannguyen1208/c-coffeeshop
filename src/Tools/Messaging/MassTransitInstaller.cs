using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Tools.Messaging.Consumers;
using Tools.Messaging.Messages;

namespace Tools.Messaging;
public static class MassTransitInstaller
{
    public static IServiceCollection AddAsyncProcessing(this IServiceCollection services, IConfiguration configuration, params Assembly[] assembliesWithConsumers)
    {
        var configData = configuration.GetSection("RabbitMQSettings");
        var brokerSettings = new BrokerSettings();
        configData.Bind(brokerSettings);

        services.AddDbContext<RegistrationDbContext>(o =>
        {
            o.UseSqlServer(configuration.GetConnectionString("Messaging"));
        });

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();

            x.AddConsumers(assembliesWithConsumers);
            x.AddConsumer<MyConsumer>();
            x.AddSagaStateMachines(entryAssembly);
            x.AddSagas(entryAssembly);
            x.AddActivities(entryAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host: brokerSettings.Host, virtualHost: "/", h =>
                {
                    h.Username(brokerSettings.Username);
                    h.Password(brokerSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });

            // Outbox
            x.AddEntityFrameworkOutbox<RegistrationDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();
            });
        });

        services.AddSingleton<ILockStatementProvider, PostgresLockStatementProvider>();
        services.AddTransient<IMessageSender, MessageSender>();

        return services;
    }

    public static IApplicationBuilder ApplyOutboxMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RegistrationDbContext>();

        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();

        return app;
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

