namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    internal static class B2CAuthConstants
    {
        public const string OpenIdConnectB2CAuthenticationScheme = "OpenID Connect B2C";

        public const string B2CCallbackPath = "/signin-oidc-b2c";

        public const string B2CPolicy = "b2cPolicy";
        public const string AcrClaimType = "http://schemas.microsoft.com/claims/authnclassreference";

        public const string ObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    }
}
