using MediatR;

namespace BookFast.Search.Indexer.Commands
{
    public class RemoveAccommodationCommand : IRequest
    {
        public int Id { get; set; }
    }
}
