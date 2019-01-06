using System.Security.Claims;

namespace ColorMix.Services.DataServices.Tests
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
        {
        }
    }
}