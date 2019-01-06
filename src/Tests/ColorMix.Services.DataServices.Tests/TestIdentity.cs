using System.Security.Claims;

namespace ColorMix.Services.DataServices.Tests
{
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims)
        {
        }
    }
}