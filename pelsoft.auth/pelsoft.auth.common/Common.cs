using Fwk.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Web;

namespace pelsoft.auth.common
{
    public class CommonHelpers
    {

        public const string CnnStringNameAD = "AD";
        public static string CnnStringAD = string.Empty;
        public const string cnnStringName_Amazon = "Amazon";
        internal static ISymetriCypher ISymetriCypher;
        public static int ExpirationSeconds = 600;
        static DateTime expirationDate = new DateTime();        

        static CommonHelpers()
        {
            expirationDate = DateTime.Now.AddSeconds(ExpirationSeconds);           
        }

        public static bool IsEncrypted()
        {
            return false;
        }

        public static SqlConnection GetCnn(string cnnName)
        {
            if (apiAppSettings.get_cnnString_byName(cnnName) == null)
            {
                throw new Fwk.Exceptions.TechnicalException("Falta cadena de conexión " + cnnName + " en el server");
            }

            System.Data.SqlClient.SqlConnection cnn;
            if (IsEncrypted())
            {
                cnn = new System.Data.SqlClient.SqlConnection(ISymetriCypher.Dencrypt(apiAppSettings.get_cnnString_byName(cnnName).cnnString));
            }
            else
            {
                cnn = new System.Data.SqlClient.SqlConnection(apiAppSettings.get_cnnString_byName(cnnName).cnnString);
            }

            return cnn;
        }

        internal static SqlConnectionStringBuilder Get_SqlConnectionStringBuilder(string cnnName)
        {

            var cnnString = string.Empty;
            if (IsEncrypted())
            {
                cnnString = ISymetriCypher.Dencrypt(apiAppSettings.get_cnnString_byName(cnnName).cnnString);
            }
            else
            {
                cnnString = apiAppSettings.get_cnnString_byName(cnnName).cnnString;
            }

            return new SqlConnectionStringBuilder(cnnString);

        }


        public static string Get_IPAddress()
        {
            IPAddress iPAddress = null;

            foreach (var a in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (IPAddress.Parse(a.ToString()).AddressFamily == AddressFamily.InterNetwork)
                    iPAddress = a;
            }

            return iPAddress.ToString();

        }


        /// <summary>
        /// Retorna si expiro el tiempo de cacheo
        /// </summary>
        public static bool Expired_ttl()
        {
            DateTime now = DateTime.Now;

            if (now > expirationDate)
            {
                expirationDate = DateTime.Now.AddSeconds(ExpirationSeconds);

                return true;
            }
            return false;

        }


        public static Boolean String_IsNull(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;

            if (str.Equals(@"''"))
                return true;

            if (str.Equals(@"undefined"))
                return true;
            
            return false;
        }

        public static string GetHash(string token)
        {
            //PasswordHasher.
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: token,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}