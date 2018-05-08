using BookFast.Search.Contracts;
using BookFast.Search.Indexer.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Search.Indexer.CommandHandlers
{
    public class RemoveAccommodationCommandHandler : IRequestHandler<RemoveAccommodationCommand>
    {
        private readonly ISearchIndexer searchIndexer;

        public RemoveAccommodationCommandHandler(ISearchIndexer searchIndexer)
        {
            this.searchIndexer = searchIndexer;
        }

        public Task Handle(RemoveAccommodationCommand message, CancellationToken cancellationToken)
        {
            return searchIndexer.DeleteAccommodationIndexAsync(message.Id);
        }
    }
}
