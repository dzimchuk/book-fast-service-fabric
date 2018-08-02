using BookFast.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookFast.ReliableEvents.CommandStack
{
    public class CommandContext
    {
        private bool owned;

        public CommandContext(IMediator mediator, ISecurityContext securityContext, ILogger<CommandContext> logger)
        {
            Mediator = mediator;
            SecurityContext = securityContext;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public ISecurityContext SecurityContext { get; }
        public ILogger Logger { get; }

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
