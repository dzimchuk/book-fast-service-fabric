using System.Linq;

namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    public class AuthenticationOptions
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }

        //public string Authority => $"{Instance}{TenantId}";
        public string Authority => $"{Instance}common";

        public string ValidIssuers { get; set; }
        public string[] ValidIssuersAsArray => 
            ValidIssuers.Split(',').Select(i => i.Trim()).ToArray();

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string PostLogoutRedirectUri { get; set; }

        public string ApiResource { get; set; }
    }
}