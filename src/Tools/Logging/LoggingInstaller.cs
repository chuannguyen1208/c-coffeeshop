using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

namespace Tools.Logging;

public static class LoggingInstaller
{
	public static IServiceCollection AddSerilogLogging(
		this IServiceCollection services,
		IConfiguration configuration,
		bool useElasticsearch = false)
	{
		var elasticConfiguration = new ElasticConfiguration();
		configuration.Bind("ElasticConfiguration", elasticConfiguration);

		var loggerConfig = new LoggerConfiguration()
			.WriteTo.Console()
			.WriteTo.File(
				path: "/logs/log-.txt",
				rollingInterval: RollingInterval.Day,
				rollOnFileSizeLimit: true,
				formatter: new CompactJsonFormatter()
			);

		if (useElasticsearch)
		{
			loggerConfig
				.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticConfiguration.Uri))
				{
					AutoRegisterTemplate = true
				});
		}


		Log.Logger = loggerConfig
			.ReadFrom.Configuration(configuration)
			.CreateLogger();

		return services;
	}
}