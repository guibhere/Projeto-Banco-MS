using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Integrador
{
    class Program
    {

        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("Ambiente");
            Console.WriteLine(environmentName);
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup(environmentName);
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetService<IIntegradorService>();
            service.IniciarIntegracao();
        }

    }
}
