using MediatR;

namespace BookFast.SeedWork.CommandStack
{
    public class CommandContext
    {
        public CommandContext(IMediator mediator)
        {
            Mediator = mediator;
        }

        public bool EventsAvailable { get; set; }

        public IMediator Mediator { get; }
    }
}
