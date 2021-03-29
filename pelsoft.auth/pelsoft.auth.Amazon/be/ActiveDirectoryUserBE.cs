using System;
using System.Collections.Generic;
using System.Text;

namespace pelsoft.auth.Amazon.be
{
 
    public class ActiveDirectoryUserBE
    {
        #region Properties


        /// <summary>
        /// Reprecenta la propiedad: userAccountControl Se utiliza para dehabilitar una cuenta.-
        /// Valor 514 dehabilita, mientras 512 mantiene la cuenta lista para logon.-
        /// </summary>
        public string UserAccountControl
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Department
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MiddleName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginNameWithDomain
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string StreetAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PostalCode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Country
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string HomePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Extension
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmailAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Company
        {
            get;
            set;
        }



        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ActiveDirectoryUserBE()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        
    }
}
