using pelsoft.auth.Amazon.be;
using pelsoft.auth.common;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace pelsoft.auth.Amazon.ActiveDirectory
{
    /// <summary>
    /// Wrapper base de para interactuar con Servicios de Directorio
    /// 
    /// </summary>
    public class DirectoryServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszUsername"></param>
        /// <param name="lpszDomain"></param>
        /// <param name="lpszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern int GetLastError();

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        //protected DirectoryEntry _directoryEntrySearchRoot;
        /// <summary>
        /// 
        /// </summary>
        protected string _LDAPPath;
        /// <summary>
        /// Domain DC name
        /// </summary>
        protected string _LDAPDomain;
        /// <summary>
        /// 
        /// </summary>
        protected string _LDAPUser;
        /// <summary>
        /// 
        /// </summary>
        protected string _LDAPPassword;

        /// <summary>
        /// Domain name ej Pelsoft-ar
        /// </summary>
        protected string _LDAPDomainName;
        /// <summary>
        /// Domain name ej Pelsoft-ar
        /// </summary>
        public string LDAPDomainName
        {
            get { return _LDAPDomainName; }

        }
        /// <summary>
        ///Ej: 
        ///LDAP://domain/DC=xxx,DC=com
        ///LDAP://CORRSF71NT13.Datacom.org/DC=Datacom,DC=org
        ///LDAP://Pc1.alcoDatacom.com.ar/OU=Datacom Sabattini,dc=alcoDatacom,dc=com,dc=ar
        /// </summary>
        public string LDAPPath
        {
            get
            {
                return _LDAPPath;
            }

        }


        /// <summary>
        ///LDAPUser property
        /// </summary>
        public string LDAPUser
        {
            get
            {
                return _LDAPUser;
            }

        }

        /// <summary>
        /// LDAPPassword property
        ///This property is reading the LDAP Password from the config file.
        /// </summary>
        public string LDAPPassword
        {
            get
            {
                return _LDAPPassword;
            }

        }


        /// <summary>
        /// Dominio
        /// </summary>
        public string LDAPDomain
        {
            get
            {
                return _LDAPDomain;
            }

        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberOf"></param>
        /// <returns></returns>
        public static List<string> GetGroupFromMemberOf(string memberOf)
        {
            int i = 0;
            string[] propAux;
            List<string> list = new List<string>();
            foreach (string prop in memberOf.Split(','))
            {
                propAux = prop.Split('=');

                if (propAux[0].CompareTo("CN") == 0)
                {
                    list.Add(propAux[1]);
                    i++;
                }

            }
            return list;
        }

        ///// <summary>
        ///// Busca la lista de dominios en una base de datos
        ///// </summary>
        ///// <param name="cnnStringName">Nombre de la cadena de coneccion configurada</param>
        ///// <returns>Lista de DomainsUrl</returns>
        //public static List<DomainUrlInfo> DomainsUrl_GetList(string cnnString)
        //{
        //    return DomainsUrl_GetList2(System.Configuration.ConfigurationManager.ConnectionStrings[cnnStringName].ConnectionString);
        //}
        ///// <summary>
        ///// Busca la lista de dominios en una base de datos.- A diferencia de DomainsUrl_GetList. Este metodo recive como parametro 
        ///// la cadena de coneccion y no su nombre de App.config
        ///// </summary>
        ///// <param name="cnnString">Cadena de coneccion</param>
        ///// <returns>Lista de DomainsUrl</returns>
        //public static List<DomainUrlInfo> DomainsUrl_GetList2(string cnnString)
        //{

        //    List<DomainUrlInfo> wDomainUrlInfoList = new List<DomainUrlInfo>();
        //    try
        //    {
        //        using (SqlDomainURLDataContext dc = new SqlDomainURLDataContext(cnnString))
        //        {
        //            IEnumerable<DomainUrlInfo> liste = from s in dc.DomainsUrls
        //                                        select new DomainUrlInfo
        //                                            {
        //                                                DomainName = s.DomainName,
        //                                                LDAPPath = s.LDAPPath,
        //                                                Id = s.DomainID,
        //                                                SiteName = s.SiteName,
        //                                                DomainDN = s.DomainDN
        //                                            };

        //            return liste.ToList<DomainUrlInfo>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Fwk.Exceptions.TechnicalException te = new Fwk.Exceptions.TechnicalException("Error al intentar obtener la lista de dominios desde la base de datos: ", ex);
        //        LDAPHelper.SetError(te);
        //        te.ErrorId = "15004";
        //        throw te;
        //    }
        //}


        /// <summary>
        /// Retorna todos los DolmainUrl por medio de un sp usp_GetDomainsUrl_All que lee de bd encriptada
        /// </summary>
        /// <param name="cnnString">Nombre de la cadena de cnn</param>
        /// <returns></returns>
        public static List<DomainUrlInfo> DomainsUrl_Get_FromSp_all(string cnnString)
        {
            string wApplicationId = string.Empty;


            DomainUrlInfo wDomainUrlInfo = null;
            List<DomainUrlInfo> list = new List<DomainUrlInfo>();
            try
            {
                var connectionString = CommonHelpers.GetCnn(cnnString).ConnectionString;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.usp_GetDomainsUrl_All", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cnn.Open();


                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            wDomainUrlInfo = new DomainUrlInfo();
                            wDomainUrlInfo.DomainDN = dr["DomainDN"].ToString();
                            wDomainUrlInfo.DomainName = dr["DomainName"].ToString();

                            wDomainUrlInfo.LDAPPath = dr["LDAPPath"].ToString();

                            wDomainUrlInfo.Pwd = dr["Pwd"].ToString();
                            wDomainUrlInfo.SiteName = dr["SiteName"].ToString();
                            wDomainUrlInfo.Usr = dr["Usr"].ToString();
                            list.Add(wDomainUrlInfo);
                        }

                    }

                    return list;


                }
            }
            catch (Exception ex)
            {
                Fwk.Exceptions.TechnicalException te = new Fwk.Exceptions.TechnicalException("Error al intentar obtener los datos del dominio desde la base de datos: ", ex);

                te.ErrorId = "15004";
                throw te;
            }
        }

        /// <summary>
        /// Retorna DolmainUrl por medio de un sp usp_GetDomainsUrl_ByDomainName que lee de bd encriptada
        /// </summary>
        /// <param name="cnnStringName">Nombre de la cadena de cnn</param>
        /// <param name="domainName">ej Allus-Ar</param>
        /// <returns></returns>
        public static DomainUrlInfo DomainsUrl_Get_FromSp(string cnnStringName, string domainName)
        {
            string wApplicationId = string.Empty;

            DomainUrlInfo wDomainUrlInfo = null;
            try
            {
                var connectionString = CommonHelpers.GetCnn(cnnStringName).ConnectionString;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.usp_GetDomainsUrl_ByDomainName", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pDomainName", domainName);
                    cnn.Open();

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            wDomainUrlInfo = new DomainUrlInfo();
                            wDomainUrlInfo.DomainDN = dr["DomainDN"].ToString();
                            wDomainUrlInfo.DomainName = dr["DomainName"].ToString();

                            wDomainUrlInfo.LDAPPath = dr["LDAPPath"].ToString();

                            wDomainUrlInfo.Pwd = dr["Pwd"].ToString();
                            wDomainUrlInfo.SiteName = dr["SiteName"].ToString();
                            wDomainUrlInfo.Usr = dr["Usr"].ToString();

                        }

                    }

                }

                return wDomainUrlInfo;


            }
            catch (Exception ex)
            {
                Fwk.Exceptions.TechnicalException te = new Fwk.Exceptions.TechnicalException("Error al intentar obtener los datos del dominio desde la base de datos: ", ex);

                te.ErrorId = "15004";
                throw te;
            }
        }


    }
    /// <summary>
    /// Proporciona una clase base para implementaciones de identificadores Win32 seguros en las que el valor 0 ó -1 indica un identificador no válido.
    /// </summary>
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}
