using Fwk.Exceptions;
using pelsoft.auth.Amazon.ActiveDirectory;
using pelsoft.auth.Amazon.be;
using pelsoft.auth.Amazon.ldap;
using pelsoft.auth.common;
using System;
using System.Collections.Generic;
using System.Linq;


namespace pelsoft.auth.Amazon.dac
{
    public class ActiveDirectoryService
    {

        public static List<DomainUrlInfo> DomainUrlInfoList;


        /// <summary>
        /// 
        /// </summary>
        static ActiveDirectoryService()
        {
            DomainUrlInfoList = RetriveDomainsUrl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public  static LoogonUserResult User_Logon(string userName, string password, string domain)
        {
            LoogonUserResult loogonUserResult = new LoogonUserResult();
            loogonUserResult.Autenticated = false;
            
            LDAPHelper _ADWrapper = new LDAPHelper(domain, CommonHelpers.CnnStringNameAD);
            TechnicalException logError = null;

            loogonUserResult.LogResult = _ADWrapper.User_Logon(userName, password, out logError).ToString();

            if (logError != null)
                loogonUserResult.ErrorMessage = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(logError);
            else
            {
                loogonUserResult.ErrorMessage = string.Empty;
                loogonUserResult.Autenticated = true;
            }


            return loogonUserResult;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal static LoogonUserResult User_Logon2(string userName, string password, string domain)
        {

            LoogonUserResult loogonUserResult = new LoogonUserResult();
            loogonUserResult.Autenticated = false;
            try
            {
                var connectionString = CommonHelpers.GetCnn(CommonHelpers.CnnStringNameAD).ConnectionString;
                LDAPHelper _ADWrapper = new LDAPHelper(domain, connectionString);
                TechnicalException logError = null;

                loogonUserResult.LogResult = _ADWrapper.User_Logon(userName, password, out logError).ToString();

                if (logError != null)
                    loogonUserResult.ErrorMessage = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(logError);
                else
                {
                    loogonUserResult.ErrorMessage = string.Empty;
                    loogonUserResult.Autenticated = true;
                }

                return loogonUserResult;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        


   

        internal static bool UserExist(string userName, string domain)
        {

            ADWrapper ad = new ADWrapper(domain, CommonHelpers.CnnStringNameAD, apiAppSettings.apiConfig.activeDirectoryImpersonate);

            bool exist = ad.User_Exists(userName);

            ad.Dispose();


            return exist;


        }






        internal static ActiveDirectoryUserBE User_Info(string userName, string domain)
        {

            ADWrapper ad = new ADWrapper(domain, CommonHelpers.CnnStringNameAD, apiAppSettings.apiConfig.activeDirectoryImpersonate);

            ADUser usr = ad.User_Get_ByName(userName);
            if (usr == null)
                return null;
            return usr.getBE();
        }

        




      

        /// <summary>
        /// desde la BD o la cache
        /// </summary>
        /// <returns></returns>
        public static List<DomainUrlInfo> RetriveDomainsUrl()
        {
            //si esta nulo o expiro refresca 
            if (DomainUrlInfoList == null || CommonHelpers.Expired_ttl())
            {
                
                DomainUrlInfoList = DirectoryServicesBase.DomainsUrl_Get_FromSp_all(CommonHelpers.CnnStringNameAD);
            }

            return DomainUrlInfoList;
        }


        /// <summary>
        /// Retorna DomainName que utiliza fwk DirectoryServices
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string Get_correct_DomainName(string siteName)
        {
            siteName = siteName.Replace("-", ".");
            
            var dom = DomainUrlInfoList.Where(p =>
            //p.DomainName.ToUpper().Equals(siteName.ToUpper()) ||
            p.SiteName.ToUpper().Equals(siteName.ToUpper())
            ).FirstOrDefault();

            if (dom == null)
            {
                throw new Exception("El dominio " + siteName + " no se encuentra configurado en la BD de reseteos");
            }
            return dom.DomainName;
        }


    }
}
