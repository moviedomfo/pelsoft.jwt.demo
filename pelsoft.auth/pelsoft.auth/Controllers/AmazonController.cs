using pelsoft.auth.Authenticators;
using pelsoft.auth.common;

namespace pelsoft.auth.Controllers
{
    public class AmazonController : AuthenticationController
    {
        public AmazonController(AmazonAuthenticator authenticator, IRefreshTokenProvider refreshTokenProvider) 
            : base(authenticator, refreshTokenProvider, "Amazon")
        {
        }
    }
}