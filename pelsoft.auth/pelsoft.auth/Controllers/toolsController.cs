using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace pelsoft.auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        [HttpGet]
        public IActionResult genJWTKey()
        {

          
            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
         
            var base64Secret = Convert.ToBase64String(key).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var resp = Ok(base64Secret);
            return resp;


        }


        [HttpGet]
        public IActionResult genJWTKeySHA256(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            var base64Secret = Convert.ToBase64String(byteHash);

            return Ok(base64Secret);
        }


        [HttpGet]
        public IActionResult decodeJWTKeySHA256(string base64Secret)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var keyByteArray = Convert.FromBase64String(Pad(base64Secret.Replace('-', '+').Replace('_', '/')));


            var securityKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            return Ok("no hace nada todavia");
        }

        private static string Pad(string text)
        {
            var padding = 3 - ((text.Length + 3) % 4);
            if (padding == 0)
            {
                return text;
            }
            return text + new string('=', padding);
        }


        [HttpGet]
        public IActionResult resolveJWTKey(string base64EncodedData)
        {
            var keyByteArray = Convert.FromBase64String(Pad(base64EncodedData.Replace('-', '+').Replace('_', '/')));
            var text = Convert.ToBase64String(keyByteArray).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            var resp = Ok("Valid RSA " + text);

            return resp;
        }


        /// <summary>
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        public IActionResult ping()
        {
            return Ok("El servicio funciona correctamente");
        }
    }
}