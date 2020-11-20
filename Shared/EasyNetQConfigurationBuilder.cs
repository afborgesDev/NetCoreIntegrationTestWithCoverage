using System;
using System.Collections.Generic;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using RabbitMQSharedConfigurations.Options;

namespace RabbitMQSharedConfigurations
{
    public static class EasyNetQConfigurationBuilder
    {
        public static ConnectionConfiguration Build(IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            var configModel = configuration.GetSection(RabbitMQConfigurationOptions.RabbitMQConfigurationOptionSection).Get<RabbitMQConfigurationOptions>();
            if (configModel == default)
                throw new ArgumentNullException("Could not find the RabbitMQ Configuration");

            return BuildConnectionConfiguration(configModel);
        }

        private static ConnectionConfiguration BuildConnectionConfiguration(RabbitMQConfigurationOptions configModel)
        {
            if (configModel is null)
                throw new ArgumentNullException(nameof(configModel));

            var host = new HostConfiguration {
                Host = configModel.HostName,
                Port = (ushort)configModel.Port
            };

            var connectionConfig = new ConnectionConfiguration {
                UserName = configModel.UserName,
                Password = configModel.Password,
                VirtualHost = configModel.VirtualHost
            };

            connectionConfig.Hosts = new List<HostConfiguration> { host };

            return connectionConfig;
        }
    }
}
