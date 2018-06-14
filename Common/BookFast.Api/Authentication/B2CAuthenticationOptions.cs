namespace BookFast.Api.Authentication
{
    public class B2CAuthenticationOptions
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }

        public string Authority => $"{Instance}{TenantId}/v2.0";

        public string Audience { get; set; }
        public string Policy { get; set; }
    }
}
