using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure
{
    internal class SecurityContextMiddleware
    {
        private readonly RequestDelegate next;

        public SecurityContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context, SecurityContext securityContext)
        {
            securityContext.Principal = context.User;
            return next(context);
        }
    }
}
