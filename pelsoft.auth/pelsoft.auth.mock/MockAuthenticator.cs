using pelsoft.auth.helpers;
using pelsoft.auth.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using pelsoft.auth.common;

namespace pelsoft.auth.Authenticators
{
    public class MockAuthenticator : IAuthenticator
    {
        private readonly IConfiguration Configuration;
        private readonly MockLogins moklogings;
        public MockAuthenticator(IConfiguration _configuration)
        {
            Configuration = _configuration;

            moklogings = new MockLogins();
            Configuration.Bind("moklogings", moklogings);
        }


        public UserEntities Authenticate(AuthenticationRequest req)
        {
            var caseInsensitiveParameters = new Dictionary<string, JsonElement>(req.AdditionalData, StringComparer.OrdinalIgnoreCase);
            var username = caseInsensitiveParameters["username"].GetString();
            var password = caseInsensitiveParameters["password"].GetString();



            var mokloging = moklogings.Where(p => p.provider == req.security_provider).FirstOrDefault();

            if (mokloging == null)
            {
                throw new AuthorizationException("No existe el provedor de seguridad " + req.security_provider);
            }

            if (password.Equals(mokloging.password) && username.Equals(mokloging.user))
            {
                var user = new
                {
                    Email = "",
                    Id = Guid.NewGuid(),
                    UserName = username
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name.ToString(), user.UserName));

                return new UserEntities
                {
                    UserId = user.Id.ToString(),
                    Claims = claimsIdentity
                };
            }
            else
            {
                throw new AuthorizationException("Usuario o password incorrecto");
            }
        }

        public UserEntities FetchUser(AuthenticationRequest req, string userId)
        {
            var mokloging = new MockLogin();
            Configuration.Bind("mokloging", mokloging);

            var user = new
            {
                Email = "",
                Id = new Guid(userId),
                UserName = mokloging.user
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name.ToString(), user.UserName));

            return new UserEntities
            {
                UserId = user.Id.ToString(),
                Claims = claimsIdentity
            };
        }
    }
}
