using MediatR;

namespace BookFast.SeedWork.CommandStack
{
    public class CommandContext
    {
        private bool owned;

        public CommandContext(IMediator mediator)
        {
            Mediator = mediator;
        }

        public IMediator Mediator { get; }

        public bool AcquireOwnership()
        {
            if (!owned)
            {
                owned = true;
                return true;
            }

            return false;
        }

        public void NotifyWhenDone()
        {
            ShouldNotify = true;
        }

        public bool ShouldNotify { get; private set; }
    }
}
