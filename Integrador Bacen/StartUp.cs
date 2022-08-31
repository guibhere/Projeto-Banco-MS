using Confluent.Kafka.DependencyInjection;
using Integrador.Helper;
using Integrador.Helper.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Integrador.Helper.SplunkLogger;

public class Startup
{
    public Startup()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json", true);
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton<IConfigurationRoot>(Configuration);

        Dictionary<string, string> KafkaConfig = Configuration.GetSection("KafkaConfig").GetChildren().ToDictionary(c => c.Key, c => c.Value);
        services.AddKafkaClient(KafkaConfig);

        var setting = Configuration.GetSection("SplunkConfig");
        services.Configure<SplunkConfig>(setting);
        services.AddScoped<ISplunkLogger, SplunkLogger>();

        services.AddSingleton<IIntegradorService,IntegradorService>();
    }
}