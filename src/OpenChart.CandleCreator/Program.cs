using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenChart.Persistence.Extensions;

namespace OpenChart.CandleCreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    services.AddDatabase(configuration);
                    services.AddHostedService<Worker>();
                });
    }
}