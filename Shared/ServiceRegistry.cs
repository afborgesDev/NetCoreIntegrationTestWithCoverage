using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitMQSharedConfigurations
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new System.ArgumentNullException(nameof(services));

            if (configuration is null)
                throw new System.ArgumentNullException(nameof(configuration));

            services.RegisterEasyNetQ(_ => EasyNetQConfigurationBuilder.Build(configuration));
            return services;
        }
    }
}