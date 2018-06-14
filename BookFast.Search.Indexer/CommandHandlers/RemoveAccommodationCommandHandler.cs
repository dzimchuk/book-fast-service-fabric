using BookFast.Search.Contracts;
using BookFast.Search.Indexer.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Search.Indexer.CommandHandlers
{
    public class RemoveAccommodationCommandHandler : AsyncRequestHandler<RemoveAccommodationCommand>
    {
        private readonly ISearchIndexer searchIndexer;

        public RemoveAccommodationCommandHandler(ISearchIndexer searchIndexer)
        {
            this.searchIndexer = searchIndexer;
        }
        
        protected override Task Handle(RemoveAccommodationCommand request, CancellationToken cancellationToken)
        {
            return searchIndexer.DeleteAccommodationIndexAsync(request.Id);
        }
    }
}
