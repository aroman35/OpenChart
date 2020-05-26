using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenChart.Application.Common;

namespace OpenChart.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(MongoSettings));
            services.AddOptions<MongoSettings>().Bind(section);
            services.AddTransient<IDbContext, OpenChartContext>();
            return services;
        }
    }
}