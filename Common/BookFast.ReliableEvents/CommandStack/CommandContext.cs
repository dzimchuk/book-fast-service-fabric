using BookFast.Security;
using MediatR;

namespace BookFast.ReliableEvents.CommandStack
{
    public class CommandContext
    {
        private bool owned;

        public CommandContext(IMediator mediator, ISecurityContext securityContext)
        {
            Mediator = mediator;
            SecurityContext = securityContext;
        }

        public IMediator Mediator { get; }
        public ISecurityContext SecurityContext { get; }

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
