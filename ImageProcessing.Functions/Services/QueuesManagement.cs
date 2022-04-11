using Azure.Messaging.ServiceBus;
using ImageProcessing.Functions.Services.Interfaces;
using System.Text.Json;

namespace ImageProcessing.Functions.Services
{
    public class QueuesManagement : IQueuesManagement
    {
        private readonly string _queueConnString;
        public QueuesManagement(string queueConnectionString)
        {
            _queueConnString = queueConnectionString ?? throw new ArgumentNullException(nameof(queueConnectionString));
        }

        public async Task<bool> SendMessageAsync<T>(T serviceMessage, string queueName)
        {
            try
            {
                // Create a queue client
                await using var client = new ServiceBusClient(_queueConnString);

                ServiceBusSender sender = client.CreateSender(queueName);

                var msgBody = JsonSerializer.Serialize(serviceMessage);

                // Create a Service Bus Message
                ServiceBusMessage msg = new(msgBody);

                await sender.SendMessageAsync(msg);

                return true;
            }
            catch (Exception ex)
            {
                // TODO handling error
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
