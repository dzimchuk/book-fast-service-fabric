namespace BookFast.ServiceBus
{
    public class ConnectionOptions
    {
        public string ConnectionString { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
    }
}
