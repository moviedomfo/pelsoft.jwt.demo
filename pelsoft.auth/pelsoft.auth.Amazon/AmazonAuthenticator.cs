using pelsoft.auth.Amazon.dac;
using pelsoft.auth.common;
using pelsoft.auth.models;
using System.Security.Claims;

namespace pelsoft.auth.Authenticators
{
    /// <summary>
    /// Clase para identificar usuarios de Amazon.
    /// </summary>
    public class AmazonAuthenticator : IAuthenticator
    {
        public UserEntities Authenticate(AuthenticationRequest req)
        {
            var domain = req.AdditionalData["domain"].GetString();
            var username = req.AdditionalData["username"].GetString();
            var password = req.AdditionalData["password"].GetString();

            var domName = ActiveDirectoryService.Get_correct_DomainName(domain);
            var log_res = ActiveDirectoryService.User_Logon(username, password, domName);

            if (log_res.LogResult == "LOGIN_USER_OR_PASSWORD_INCORRECT")
            {
                throw new AuthorizationException("El usuario y/o contraseña es incorrecto  ");
            }

            if (log_res.LogResult == "LOGIN_USER_DOESNT_EXIST")
            {
                throw new AuthorizationException("El usuario no existe en el dominio  ");
            }

            return FetchUser(req, domain + "/" + username);
        }

        public UserEntities FetchUser(AuthenticationRequest req, string userId)
        {
            var parts = userId.Split("/");
            var domName = parts[0];
            var username = parts[1];

            var empleadoBE = AmazonDAC.Get(username, domName);
            if (empleadoBE == null)
            {
                throw new AuthorizationException("Usuario no registrado en Amazon");
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name.ToString(), empleadoBE.UserName));

            return new UserEntities
            {
                Claims = claimsIdentity,
                UserId = userId
            };
        }
    }
}
