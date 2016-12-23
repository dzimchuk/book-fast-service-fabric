using Microsoft.AspNetCore.Builder;

namespace BookFast.Security.AspNetCore
{
    public static class SecurityContextExtensions
    {
        public static void UseSecurityContext(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SecurityContextMiddleware>();
        } 
    }
}