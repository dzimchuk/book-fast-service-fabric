using BookFast.SeedWork.CommandStack;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    internal class NotificationPublisher : INotificationHandler<EventsAvailableNotification>
    {
        private readonly QueueClient queueClient;

        public NotificationPublisher(IOptions<ConnectionOptions> options)
        {
            queueClient = new QueueClient(options.Value.NotificationQueueConnection, options.Value.NotificationQueueName, ReceiveMode.PeekLock, RetryPolicy.Default);
        }
        public Task Handle(EventsAvailableNotification notification, CancellationToken cancellationToken)
        {
            if (queueClient == null)
            {
                return Task.CompletedTask;
            }

            var jsonMessage = JsonConvert.SerializeObject(notification);

            var message = new Message
            {
                MessageId = notification.Id.ToString(),
                Body = Encoding.UTF8.GetBytes(jsonMessage),
                Label = notification.GetType().Name
            };

            return queueClient.SendAsync(message);
        }
    }
}
