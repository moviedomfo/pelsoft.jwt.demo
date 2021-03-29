using Fwk.Security.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using fwkSec = fwk.security.identity.core;
using pelsoft.auth.models;
namespace pelsoft.auth
{
    /// <summary>
    /// Clase para administrar tokens JWT
    /// </summary>
    public class TokenGenerator
    {
        /// <summary>
        /// Genera un token de acceso para el usuario <c>secUser</c>, segun
        /// la configuracion de <c>securityProvider</c>.
        /// 
        /// Provee el claim <c>Name</c>.
        /// </summary>
        /// <param name="claimsIdentity">Claims a almacenar en el token</param>B
        /// <param name="securityProvider">Nombre de la configuracion de seguridad a utilizar</param>
        /// <returns>Un JWT</returns>
        public static string GenerateToken(ClaimsIdentity claimsIdentity, string securityProvider)
        {
            var provider = fwkSec.SecurityManager.get_secConfig().GetByName(securityProvider);
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(provider.audienceSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtSecToken = new JwtSecurityToken(
                issuer: provider.issuer,
                audience: provider.audienceId,
                claims: claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddSeconds(Convert.ToInt32(provider.expires)),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtTokenString = tokenHandler.WriteToken(jwtSecToken);

            return jwtTokenString;
        }

        /// <summary>
        /// Este metodo incluye Roles . Para el caso donde tengammos un componente DAC o API donde podamos 
        /// obtener un onjeto SecurityUserBE y tambien el Id de la persona fisica
        /// 
        /// En el demo no se desarrolla la obtenicion de estos datos queran dado que el modelado 
        /// de Usuario-->Person Pathern sera de otro capítulo
        /// </summary>
        /// <param name="securityProvider"></param>
        /// /// <param name="personId"></param>
        /// <returns></returns>
        public static string GenerateToken2(pelsoft.auth.models.SecurityUserBE secUser, Guid? personId, string securityProvider)
        {
            var provider = fwkSec.SecurityManager.get_secConfig().GetByName(securityProvider);
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(provider.audienceSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name.ToString(), secUser.UserName) });
            if (secUser != null)
            {

                claimsIdentity.AddClaim(new Claim("userId", secUser.UserId.ToString()));
                claimsIdentity.AddClaim(new Claim("personId", personId.ToString()));

                if (!string.IsNullOrEmpty(secUser.Lastname))
                    claimsIdentity.AddClaim(new Claim("lastName", secUser.Lastname.ToString()));
                if (!string.IsNullOrEmpty(secUser.FirstName))
                    claimsIdentity.AddClaim(new Claim("firstName", secUser.FirstName.ToString()));

                claimsIdentity.AddClaim(new Claim("fullname", secUser.ToString()));
                claimsIdentity.AddClaim(new Claim("email", secUser.Email));
                if (secUser.Roles != null)
                {

                    string jsonRoles = Fwk.HelperFunctions.SerializationFunctions.SerializeObjectToJson<string[]>(secUser.Roles);
                    claimsIdentity.AddClaim(new Claim("roles", jsonRoles));
                }


            }

            var jwtSecToken = new JwtSecurityToken(
                issuer: provider.issuer,
                audience: provider.audienceId,
                claims: claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddSeconds(Convert.ToInt32(provider.expires)),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtTokenString = tokenHandler.WriteToken(jwtSecToken);

            return jwtTokenString;
        }
    }
}
