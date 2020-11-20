namespace EntryPoint.Processor.Options
{
    public class RabbitMqOptions
    {
        public const string RabbitMqConfigSection = "RabbitMq";
        public string HostName { get; set; }

        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }
    }
}