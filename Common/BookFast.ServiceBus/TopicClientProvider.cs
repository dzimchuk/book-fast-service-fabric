using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;

namespace BookFast.ServiceBus
{
    internal class TopicClientProvider
    {
        private readonly TopicClient topicClient;

        public TopicClientProvider(IOptions<ConnectionOptions> options)
        {
            if (!string.IsNullOrWhiteSpace(options.Value.ConnectionString))
            {
                topicClient = new TopicClient(options.Value.ConnectionString, options.Value.TopicName, RetryPolicy.Default);
            }
        }

        public TopicClient Client => topicClient;
    }
}
