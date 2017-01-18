using Microsoft.ServiceFabric.Services.Communication.Client;

namespace BookFast.ServiceFabric.Communication
{
    public interface IPartitionClientFactory<TCommunicationClient> where TCommunicationClient : ICommunicationClient
    {
        ServicePartitionClient<TCommunicationClient> CreatePartitionClient();
    }
}
