using Fwk.Exceptions;
using Fwk.HelperFunctions;
using Fwk.Security.ActiveDirectory;
using Fwk.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace microsoft.resource.api.common
{
    public class CommonHelpers
    {
        
        internal const string cnnStringName_microsoft = "microsoft"; 
        internal static ISymetriCypher ISymetriCypher;
        public static int ExpirationSeconds = 600;
      

        

        static CommonHelpers()
        {
           

        }




        public static string getMessageException(Exception ex)
        {
            string msg = string.Empty;
            //if (ex.GetType() == typeof(HttpResponseException))
            //        msg = ex.Message;
            //return msg = ex.Message;

            if (ex.GetType() == typeof(TechnicalException))
            {
                var te = ex as TechnicalException;

                msg = te.Message;
                //return msg;
            }
            if (ex.InnerException != null)
            {

                msg = msg + ex.InnerException.Message;
                if (ex.InnerException.GetType() == typeof(System.Net.Sockets.SocketException))
                {
                    var e = ex.InnerException as System.Net.Sockets.SocketException;
                    if (e.ErrorCode == 10060)
                        msg = "La url no es accesible " + Environment.NewLine + msg;
                }
                //if (ex.InnerException.GetType() == typeof(WebExcepcion))
                //{
                //    var e = ex.InnerException as System.Net.Sockets.SocketException;
                //    if (e.ErrorCode == 10060)
                //        msg = "WAPI wapiAppSettings.apiConfig.apiDomain no es accesible " + Environment.NewLine + msg;
                //}
            }

            else
                msg = ex.Message;

            return msg;
        }


        public static HttpClientHandler getProxy_HttpClientHandler()
        {
            HttpClientHandler httpClientHandler = null;
            if (apiAppSettings.apiConfig.proxyEnabled)
            {
                var proxy = new WebProxy()
                {
                    Address = new Uri(string.Format("http://{0}:{1}", apiAppSettings.apiConfig.proxyName, apiAppSettings.apiConfig.proxyPort)),
                    //BypassOnLocal = false,
                    UseDefaultCredentials = true

                    // *** These creds are given to the proxy server, not the web server ***
                    //Credentials = new NetworkCredential(
                    //    userName: wapiAppSettings.apiConfig.proxyUser,
                    //    password: wapiAppSettings.apiConfig.proxyPassword)
                };


                httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                    UseProxy = true,
                    PreAuthenticate = false,
                    UseDefaultCredentials = true

                };

            }

            return httpClientHandler;
        }


        /// <summary>
        /// Converts a DateTime to the long representation which is the number of seconds since the unix epoch.
        /// Epoch (UNIX Epoch time) : It is the number of seconds that have elapsed since 00:00:00 Thursday, 1 January 1970,[2] Coordinated Universal Time (UTC), minus leap seconds. 
        /// </summary>
        /// <param name="dateTime">A DateTime to convert to epoch time.</param>
        /// <returns>The long number of seconds since the unix epoch.</returns>
        public static long ToEpoch(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }



        /// <summary>
        /// Converts a long representation of time since the unix epoch to a DateTime.
        /// </summary>
        /// <param name="epoch">The number of seconds since Jan 1, 1970.
        /// Epoch (UNIX Epoch time) : It is the number of seconds that have elapsed since 00:00:00 Thursday, 1 January 1970,[2] Coordinated Universal Time (UTC), minus leap seconds. </param>
        /// <returns>A DateTime representing the time since the epoch.</returns>
        public static DateTime FromEpoch(long epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddMilliseconds(epoch);
        }


        public static void SendMail(string subjet, string body, string from, string to, string accountConfig)
        {
            if (string.IsNullOrEmpty(from) || to.Length == 0)
                return;

            //Crea el nuevo correo electronico con el cuerpo del mensaje y el asutno.
            MailMessage wMailMessage = new MailMessage() { Body = body, Subject = subjet };
            wMailMessage.IsBodyHtml = true;

            // Add image attachment from local disk
            //string file = System.Web.Hosting.HostingEnvironment.MapPath("~/content/files/Email_Forgot_Password.html");
            Attachment oAttachment = new Attachment("img_header_content_id.jpg");

            // Specifies the attachment as an embedded image
            // contentid can be any string.
            string contentID = "img_header_content_id";
            oAttachment.ContentId = contentID;
            wMailMessage.Attachments.Add(oAttachment);
            //Asigna el remitente del mensaje de acuerdo a direccion obtenida en el archivo de configuracion.
            wMailMessage.From = new MailAddress(from);
            //Asigna los destinatarios del mensaje de acuerdo a las direcciones obtenidas en el archivo de configuracion.
            //foreach (string recipient in MailRecipients)
            //{
            wMailMessage.To.Add(new MailAddress(to));
            //}

            SmtpClient wSmtpClient = new SmtpClient(apiAppSettings.apiConfig.api_mail.smtp, apiAppSettings.apiConfig.api_mail.port);
            wSmtpClient.EnableSsl = apiAppSettings.apiConfig.api_mail.enableSsl;
            NetworkCredential cred = new NetworkCredential(apiAppSettings.apiConfig.api_mail.user, apiAppSettings.apiConfig.api_mail.pwd);
            wSmtpClient.Credentials = cred;



            //Envia el correo electronico.
            try
            {

                wSmtpClient.Send(wMailMessage);
            }
            catch (Exception ex)
            {
                string exErr = ex.Message;
            }
        }

        //public static bool IsEncrypted(System.Configuration.Configuration config)
        //{
        //    if (config.AppSettings.Settings["crypt"] == null)
        //        return false;
        //    else
        //        return Convert.ToBoolean(config.AppSettings.Settings["crypt"].Value);
        //}

        public static bool IsEncrypted()
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["crypt"] == null)
            return false;
            //else
            //    return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["crypt"]);
        }

        public static SqlConnection GetCnn(string cnnName)
        {

            if (apiAppSettings.get_cnnString_byName(cnnName) == null)
            {
                throw new Fwk.Exceptions.TechnicalException("Falta cadena de conexion " + cnnName + " en el server");
            }
            System.Data.SqlClient.SqlConnection cnn = null;
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


       


       
   
    
    }


}