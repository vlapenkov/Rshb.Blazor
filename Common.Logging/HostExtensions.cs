using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

namespace Common.Logging;

public static class HostExtensions
{
    public static IHostBuilder UseLogging(this IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(b =>
        {
            var jsonSource = new JsonStreamConfigurationSource 
            {
                Stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"{nameof(Common)}.{nameof(Logging)}.settings.json")
            };
            
            b.Sources.Insert(0, jsonSource);
        });

        return builder.UseSerilog((context, services, configuration) =>
        {
#if LogToElasticsearch
            ConfigureElasticsearch(context.Configuration);
#endif
            var c = configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.WithThreadId()
                .Enrich.FromLogContext();
        });
    }

#if LogToElasticsearch
    private static void ConfigureElasticsearch(IConfiguration configuration)
    {
        var elasticArgs = configuration.GetSection("Serilog:WriteTo:Elasticsearch:Args");

        var assembly = Assembly.GetEntryAssembly();
        string assemblyName = assembly.GetName().Name;

        string paramName = assemblyName.Split('.')[0].ToUpper() + "_ES_NODES";
        string nodeUris = configuration[paramName];
        if (string.IsNullOrWhiteSpace(nodeUris))
            throw new ArgumentNullException(paramName, "Не указан адрес Elasticsearch");

        elasticArgs["nodeUris"] = nodeUris;

        string env = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration.ToLower();
        string serviceName = assemblyName.ToLower().Replace('.', '_');

        elasticArgs["indexFormat"] = $"{env}_{serviceName}_log_{{0:yyyy_MM}}";
        elasticArgs["bufferBaseFilename"] = Path.Combine(Path.GetTempPath(), "taxi_es", serviceName);
    }
#endif
}