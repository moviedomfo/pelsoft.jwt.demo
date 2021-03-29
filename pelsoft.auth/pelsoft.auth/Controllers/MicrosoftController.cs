using pelsoft.auth.Authenticators;
using pelsoft.auth.common;

namespace pelsoft.auth.Controllers
{
    public class MicrosoftController : AuthenticationController
    {
        public MicrosoftController(MockAuthenticator authenticator, IRefreshTokenProvider refreshTokenProvider) : base(authenticator, refreshTokenProvider, "Microsoft")
        {
        }
    }
}