using System.Security.Claims;

namespace MainApp.Web.Tests
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            //provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "User"));
            return provider;
        }
    }
}
