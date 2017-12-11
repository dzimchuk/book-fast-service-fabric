namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    public class B2CAuthenticationOptions
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }

        public string Authority => $"{Instance}{TenantId}/v2.0";
        public string GetAuthority(string policy) => $"{Instance}tfp/{TenantId}/{policy}/v2.0";

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string PostLogoutRedirectUri { get; set; }
        public string ApiIdentifier { get; set; }
    }
}
