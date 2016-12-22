using System.Linq;

namespace BookFast.Facility.Infrastructure.Authentication
{
    public class AuthenticationOptions
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string Audience { get; set; }

        //public string Authority => $"{Instance}{TenantId}";
        public string Authority => $"{Instance}common";

        public string ValidIssuers { get; set; }
        public string[] ValidIssuersAsArray =>
            ValidIssuers.Split(',').Select(i => i.Trim()).ToArray();
    }
}