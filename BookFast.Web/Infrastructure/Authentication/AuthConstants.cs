namespace BookFast.Web.Infrastructure.Authentication
{
    internal static class AuthConstants
    {
        public const string OpenIdConnectOrganizationalAuthenticationScheme = "OpenID Connect";
        public const string OpenIdConnectB2CAuthenticationScheme = "OpenID Connect B2C";

        public const string OrganizationalCallbackPath = "/signin-oidc";
        public const string B2CCallbackPath = "/signin-oidc-b2c";

        public const string B2CPolicy = "b2cPolicy";
        public const string AcrClaimType = "http://schemas.microsoft.com/claims/authnclassreference";
    }
}
