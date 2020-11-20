using System;
using System.Text;
using System.Threading;
using EasyNetQ;
using EasyNetQ.Topology;
using EntryPoint.Processor;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace EntryPoint.Integration.Test
{
    public class HappyPathEntryPointTest
    {
        private const string EntryMessageIntegrationTest = nameof(EntryMessageIntegrationTest);
        private readonly IAdvancedBus bus;
        private readonly ConnectionConfiguration connectionConfiguration;
        private IQueue InitializedIntegrationQueue;
        private MessageReceived messageReceived;

        public HappyPathEntryPointTest(IAdvancedBus bus, ConnectionConfiguration connectionConfiguration)
        {
            this.bus = bus;
            this.connectionConfiguration = connectionConfiguration;
            SetupConsumer();
        }

        [Fact]
        public void ShouldBeTrue()
        {
            this.bus.Consume(this.InitializedIntegrationQueue, HandleMessageAsync);

            var waitCount = 0;
            while (waitCount < 5)
            {
                if (this.messageReceived != null)
                    break;
                Thread.Sleep(TimeSpan.FromSeconds(5));
                waitCount++;
            }

            messageReceived.Should().NotBeNull();
            messageReceived.Message.Should().BeEquivalentTo($"{EntryMessageIntegrationTest}-EntryPointProcessed-");
        }

        private void HandleMessageAsync(byte[] body, MessageProperties messageProperties, MessageReceivedInfo messageReceivedInfo)
        {
            if (body.Length == 0)
                return;

            var decodedBody = Encoding.UTF8.GetString(body);
            messageReceived = JsonConvert.DeserializeObject<MessageReceived>(decodedBody);
        }

        private void SetupConsumer()
        {
            const string routingKey = "message.initialized";
            var exchange = this.bus.ExchangeDeclare("general.topic", ExchangeType.Topic);

            this.InitializedIntegrationQueue = this.bus.QueueDeclare("message.initialized-integration-test");
            this.bus.Bind(exchange, this.InitializedIntegrationQueue, routingKey);

            var toSendMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new MessageReceived { Message = EntryMessageIntegrationTest }));
            this.bus.Publish(exchange, "message.received", true, new MessageProperties() {
                DeliveryMode = MessageDeliveryMode.Persistent,
                Expiration = TimeSpan.FromSeconds(5).TotalMilliseconds.ToString()
            }, toSendMessage);
        }
    }
}
