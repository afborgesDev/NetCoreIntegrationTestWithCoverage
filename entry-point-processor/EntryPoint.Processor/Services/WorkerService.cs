using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint.Processor.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly ILogger<WorkerService> logger;
        private readonly IAdvancedBus bus;
        private IQueue? queue;
        private IExchange exchange;

        public WorkerService(ILogger<WorkerService> logger, IAdvancedBus bus)
        {
            this.logger = logger;
            this.bus = bus;
        }

        public Task ConsumeAsync()
        {
            PrepareToConsume();
            this.logger.LogInformation("Consuming");
            this.bus.Consume(this.queue, HandleMessageAsync);
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(byte[] body, MessageProperties messageProperties, MessageReceivedInfo messageReceivedInfo)
        {
            if (body.Length == 0)
                return;

            this.logger.LogInformation("Received Message");

            var decodedBody = Encoding.UTF8.GetString(body);
            var messageReceived = JsonConvert.DeserializeObject<MessageReceived>(decodedBody);

            await PublishMessageEnriched(messageProperties, messageReceived).ConfigureAwait(false);
        }

        private async Task PublishMessageEnriched(MessageProperties messageProperties, MessageReceived messageReceived)
        {
            this.logger.LogInformation("Message enriched");

            messageReceived.Message += "-EntryPointProcessed-";
            messageProperties.DeliveryMode = MessageDeliveryMode.Persistent;
            var toSendMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageReceived));

            this.logger.LogInformation("Send to internal process");
            await this.bus.PublishAsync(this.exchange, "message.initialized", true, messageProperties, toSendMessage).ConfigureAwait(false);
        }

        private void PrepareToConsume()
        {
            PolicyForRabbitCreation.MainPolicy.Execute(() => this.exchange = this.bus.ExchangeDeclare("general.topic", ExchangeType.Topic));

            PolicyForRabbitCreation.MainPolicy.Execute(() =>
            {
                this.queue = this.bus.QueueDeclare("message.entrypoint");
                this.bus.Bind(this.exchange, this.queue, "message.received");
            });
        }
    }
}