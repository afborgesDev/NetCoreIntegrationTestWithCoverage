namespace RabbitMQSharedConfigurations.Options
{
    public class RabbitMQConfigurationOptions
    {
        public const string RabbitMQConfigurationOptionSection = "RabbitMQ";

        public string? HostName { get; set; }

        public string? VirtualHost { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public int Port { get; set; }
    }
}
