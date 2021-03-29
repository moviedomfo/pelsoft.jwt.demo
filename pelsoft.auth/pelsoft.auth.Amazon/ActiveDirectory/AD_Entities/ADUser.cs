using pelsoft.auth.Amazon.be;
using pelsoft.auth.Amazon.ldap;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace pelsoft.auth.Amazon.ActiveDirectory
{
    /// <summary>
    /// Clase que representa un usuario de active directory.- 
    /// 1.       This class has read only properties for fetching First Name, Last Name, City, Login Name etc.
    /// 2.       Constructor of the class is taking one parameter of type DirectoryEntry class.
    /// 3.       In Constructor all the information about ADUser is getting fetched using static class ADProperties.
    /// 4.       There are two static functions inside this class. GetUser and GetProperty
    /// 5.       Get Property is returning a string which holds property of AD User.
    /// 6.       GetUser static function is returning an ADUser. 
    /// </summary>
    public class ADUser
    {
        #region Properties
        private LoginResult _LoginResult;
        /// <summary>
        /// 
        /// </summary>
        public LoginResult LoginResult
        {
            get { return _LoginResult; }
            set { _LoginResult = value; }
        }
        private readonly string _firstName;

        private readonly string _middleName;

        private readonly string _lastName;

        private readonly string _loginName;

        private readonly string _loginNameWithDomain;

        private readonly string _streetAddress;

        private readonly string _city;

        private readonly string _state;

        private readonly string _postalCode;

        private readonly string _country;

        private readonly string _homePhone;

        private readonly string _extension;

        private readonly string _mobile;

        private readonly string _fax;

        private readonly string _emailAddress;

        private readonly string _title;

        private readonly string _company;

        private readonly string _manager;

        private readonly string _managerName;

        private readonly string _department;
        private readonly string _UserAccountControl;

        /// <summary>
        /// Reprecenta la propiedad: userAccountControl Se utiliza para dehabilitar una cuenta.-
        /// Valor 514 dehabilita, mientras 512 mantiene la cuenta lista para logon.-
        /// </summary>
        public string UserAccountControl
        {
            get { return _UserAccountControl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Department
        {
            get { return _department; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginName
        {
            get { return _loginName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginNameWithDomain
        {
            get { return _loginNameWithDomain; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StreetAddress
        {
            get { return _streetAddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PostalCode
        {
            get { return _postalCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Country
        {
            get { return _country; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HomePhone
        {
            get { return _homePhone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Extension
        {
            get { return _extension; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile
        {

            get { return _mobile; }

        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            get { return _fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmailAddress
        {
            get { return _emailAddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Company
        {
            get { return _company; }
        }

        readonly List<string> _Groups;


        /// <summary>
        /// 
        /// </summary>
        public string ManagerName
        {
            get { return _managerName; }

        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="directoryUser"></param>
        public ADUser(DirectoryEntry directoryUser)
        {



            string domainAddress;
            string userPrincipalName = ADWrapper.GetProperty(directoryUser, ADProperties.USERPRINCIPALNAME);
            string domainName;
            _UserAccountControl = ADWrapper.GetProperty(directoryUser, ADProperties.USERACCOUNTCONTROL);
            _firstName = ADWrapper.GetProperty(directoryUser, ADProperties.FIRSTNAME);

            _middleName = ADWrapper.GetProperty(directoryUser, ADProperties.MIDDLENAME);

            _lastName = ADWrapper.GetProperty(directoryUser, ADProperties.LASTNAME);

            _loginName = ADWrapper.GetProperty(directoryUser, ADProperties.LOGINNAME);



            if (!string.IsNullOrEmpty(userPrincipalName))
            {
                domainAddress = userPrincipalName.Split('@')[1];
            }
            else
            {
                domainAddress = string.Empty;
            }

            if (!string.IsNullOrEmpty(domainAddress))
            {

                domainName = domainAddress.Split('.').First();

            }

            else
            {

                domainName = string.Empty;

            }

            _loginNameWithDomain = string.Format(@"{0}\{1}", domainName, _loginName);

            _streetAddress = ADWrapper.GetProperty(directoryUser, ADProperties.STREETADDRESS);

            _city = ADWrapper.GetProperty(directoryUser, ADProperties.CITY);

            _state = ADWrapper.GetProperty(directoryUser, ADProperties.STATE);

            _postalCode = ADWrapper.GetProperty(directoryUser, ADProperties.POSTALCODE);

            _country = ADWrapper.GetProperty(directoryUser, ADProperties.COUNTRY);

            _company = ADWrapper.GetProperty(directoryUser, ADProperties.COMPANY);

            _department = ADWrapper.GetProperty(directoryUser, ADProperties.DEPARTMENT);

            _homePhone = ADWrapper.GetProperty(directoryUser, ADProperties.HOMEPHONE);

            _extension = ADWrapper.GetProperty(directoryUser, ADProperties.EXTENSION);

            _mobile = ADWrapper.GetProperty(directoryUser, ADProperties.MOBILE);

            _fax = ADWrapper.GetProperty(directoryUser, ADProperties.FAX);

            _emailAddress = ADWrapper.GetProperty(directoryUser, ADProperties.EMAILADDRESS);

            _title = ADWrapper.GetProperty(directoryUser, ADProperties.TITLE);

            _manager = ADWrapper.GetProperty(directoryUser, ADProperties.MANAGER);

            if (!string.IsNullOrEmpty(_manager))
            {

                string[] managerArray = _manager.Split(',');

                _managerName = managerArray[0].Replace("CN=", "");

            }

            // NOTA: En este código se confunden las propiedades LockedOut con Disabled... supuestamente se obtiene con la pripiedad
            // LockedOutTime pero no funciona
            //_LockedOut = Convert.ToBoolean(directoryUser.InvokeGet("IsAccountLocked")); 

            //_LoginResult = ADWrapper.User_Get_LoginResult(directoryUser);

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resultUserUser"></param>
        public ADUser(SearchResult resultUser)
        {



            string domainAddress;
            string userPrincipalName = ADWrapper.GetProperty(resultUser, ADProperties.USERPRINCIPALNAME);
            string domainName;
            _UserAccountControl = ADWrapper.GetProperty(resultUser, ADProperties.USERACCOUNTCONTROL);
            _firstName = ADWrapper.GetProperty(resultUser, ADProperties.FIRSTNAME);

            _middleName = ADWrapper.GetProperty(resultUser, ADProperties.MIDDLENAME);

            _lastName = ADWrapper.GetProperty(resultUser, ADProperties.LASTNAME);

            _loginName = ADWrapper.GetProperty(resultUser, ADProperties.LOGINNAME);



            if (!string.IsNullOrEmpty(userPrincipalName))
            {
                domainAddress = userPrincipalName.Split('@')[1];
            }
            else
            {
                domainAddress = string.Empty;
            }

            if (!string.IsNullOrEmpty(domainAddress))
            {

                domainName = domainAddress.Split('.').First();

            }

            else
            {

                domainName = string.Empty;

            }

            _loginNameWithDomain = string.Format(@"{0}\{1}", domainName, _loginName);

            _streetAddress = ADWrapper.GetProperty(resultUser, ADProperties.STREETADDRESS);

            _city = ADWrapper.GetProperty(resultUser, ADProperties.CITY);

            _state = ADWrapper.GetProperty(resultUser, ADProperties.STATE);

            _postalCode = ADWrapper.GetProperty(resultUser, ADProperties.POSTALCODE);

            _country = ADWrapper.GetProperty(resultUser, ADProperties.COUNTRY);

            _company = ADWrapper.GetProperty(resultUser, ADProperties.COMPANY);

            _department = ADWrapper.GetProperty(resultUser, ADProperties.DEPARTMENT);

            _homePhone = ADWrapper.GetProperty(resultUser, ADProperties.HOMEPHONE);

            _extension = ADWrapper.GetProperty(resultUser, ADProperties.EXTENSION);

            _mobile = ADWrapper.GetProperty(resultUser, ADProperties.MOBILE);

            _fax = ADWrapper.GetProperty(resultUser, ADProperties.FAX);

            _emailAddress = ADWrapper.GetProperty(resultUser, ADProperties.EMAILADDRESS);

            _title = ADWrapper.GetProperty(resultUser, ADProperties.TITLE);

            _manager = ADWrapper.GetProperty(resultUser, ADProperties.MANAGER);

            if (!string.IsNullOrEmpty(_manager))
            {

                string[] managerArray = _manager.Split(',');

                _managerName = managerArray[0].Replace("CN=", "");

            }

            //_LockedOut = Convert.ToBoolean(resultUser.GetDirectoryEntry().InvokeGet("IsAccountLocked")); 

            //_LoginResult = ADWrapper.User_Get_LoginResult(resultUser);

        }


     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pSource"></param>
        /// <returns></returns>
        public static List<ADUser> FilterByName(string pName, List<ADUser> pSource)
        {
            if (pSource == null) return null;

            return (List<ADUser>)
                pSource.Where<ADUser>(p => (
                    p.LoginName.StartsWith(pName, StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrEmpty(pName)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActiveDirectoryUserBE getBE()
        {
            ActiveDirectoryUserBE userBE = new ActiveDirectoryUserBE() ;
            userBE.Company = this.Company;
            userBE.Country = this.Country;
            userBE.Department = this.Department;
            userBE.EmailAddress = this.EmailAddress;
            userBE.Extension = this.Extension;
            userBE.Fax = this.Fax;
            userBE.FirstName = this.FirstName;
            userBE.HomePhone = this.HomePhone;
            userBE.LastName = this.LastName;
            userBE.LoginName = this.LoginName;
            userBE.LoginNameWithDomain = this.LoginName;
            userBE.MiddleName = this.MiddleName;
            userBE.Mobile = this.Mobile;
            userBE.PostalCode = this.PostalCode;
            userBE.State = this.State;
            userBE.StreetAddress = this.StreetAddress;
            userBE.UserAccountControl = this.UserAccountControl;
            userBE.Title = this.Title;

            return userBE;

        }
    }

}


