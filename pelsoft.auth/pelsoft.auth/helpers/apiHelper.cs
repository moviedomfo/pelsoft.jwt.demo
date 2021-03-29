
using pelsoft.auth.common;

using Fwk.Exceptions;
using Fwk.HelperFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace pelsoft.auth.helpers
{
    public class apiHelper
    {
        


        public static string getMessageException(Exception ex)
        {
            string msg = string.Empty;
            //if (ex.GetType() == typeof(HttpResponseException))
            //        msg = ex.Message;
            //return msg = ex.Message;

            if(ex.GetType() == typeof(TechnicalException))
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
                        msg =   "La url no es accesible " + Environment.NewLine + msg;
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


       
    }
}
