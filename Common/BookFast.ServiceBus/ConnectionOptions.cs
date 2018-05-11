namespace BookFast.ServiceBus
{
    internal class ConnectionOptions
    {
        public string ConnectionString { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
    }
}
