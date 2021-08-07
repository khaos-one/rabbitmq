using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khaos.RabbitMq
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqChannelFactory(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .Configure<RabbitMqChannelFactoryOptions>(configuration)
                .AddSingleton<IRabbitMqChannelFactory, RabbitMqChannelFactory>();
    }
}