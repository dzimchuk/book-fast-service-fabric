using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookFast.Security.AspNetCore
{
    internal class SecurityContextMiddleware
    {
        private readonly RequestDelegate next;

        public SecurityContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context, ISecurityContext securityContext)
        {
            var acceptor = securityContext as ISecurityContextAcceptor;
            if (acceptor != null)
                acceptor.Principal = context.User;

            return next(context);
        }
    }
}