namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    internal static class AuthConstants
    {
        public const string OpenIdConnectOrganizationalAuthenticationScheme = "OpenID Connect";

        public const string OrganizationalCallbackPath = "/signin-oidc";

        public const string ObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    }
}
