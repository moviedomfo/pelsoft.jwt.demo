using System;
using System.Collections.Generic;
using System.Text;

namespace pelsoft.auth.Amazon.ActiveDirectory
{
    /// <summary>
    /// This class will have the properties corresponding to the information of the AD User.  
    /// This is a static class. This class is having all the properties as constant string for ADUser.  
    /// This class is giving readable name to all the properties of user details. 
    /// </summary>
    public static class ADProperties
    {
        /// <summary>
        /// 
        /// </summary>
        public const string OBJECTCLASS = "objectClass";
        /// <summary>
        /// 
        /// </summary>
        public const string CONTAINERNAME = "cn";
        /// <summary>
        /// 
        /// </summary>
        public const string LASTNAME = "sn";
        /// <summary>
        /// 
        /// </summary>
        public const string COUNTRYNOTATION = "c";
        /// <summary>
        /// 
        /// </summary>
        public const string CITY = "l";
        /// <summary>
        /// 
        /// </summary>
        public const string STATE = "st";
        /// <summary>
        /// 
        /// </summary>
        public const string TITLE = "title";
        /// <summary>
        /// 
        /// </summary>
        public const string POSTALCODE = "postalCode";
        /// <summary>
        /// 
        /// </summary>
        public const string PHYSICALDELIVERYOFFICENAME = "physicalDeliveryOfficeName";

        /// <summary>
        /// givenName
        /// </summary>
        public const string FIRSTNAME = "givenName";

        /// <summary>
        /// initials
        /// </summary>
        public const string MIDDLENAME = "initials";

        /// <summary>
        /// DN Es uno de los mas importantes atributos LDAP
        /// CN=Jay Jamieson, OU= Newport,DC=cp,DC=com
        /// CN=GS_CalidadTP_R,OU=Avanzados,OU=Analistas,OU=Seguridad,DC=Datacom,DC=org
        /// </summary>
        public const string DISTINGUISHEDNAME = "distinguishedName";
        /// <summary>
        /// 
        /// </summary>
        public const string INSTANCETYPE = "instanceType";
        /// <summary>
        /// 
        /// </summary>
        public const string WHENCREATED = "whenCreated";
        /// <summary>
        /// 
        /// </summary>
        public const string WHENCHANGED = "whenChanged";
        /// <summary>
        /// 
        /// </summary>
        public const string DISPLAYNAME = "displayName";

        /// <summary>
        /// uSNCreated
        /// </summary>
        public const string USNCREATED = "uSNCreated";
        /// <summary>
        /// 
        /// </summary>
        public const string MEMBEROF = "memberOf";
        /// <summary>
        /// 
        /// </summary>
        public const string USNCHANGED = "uSNChanged";
        /// <summary>
        /// 
        /// </summary>
        public const string COUNTRY = "co";
        /// <summary>
        /// 
        /// </summary>
        public const string DEPARTMENT = "department";
        /// <summary>
        /// 
        /// </summary>
        public const string COMPANY = "company";
        /// <summary>
        /// 
        /// </summary>
        public const string PROXYADDRESSES = "proxyAddresses";
        /// <summary>
        /// 
        /// </summary>
        public const string STREETADDRESS = "streetAddress";
        /// <summary>
        /// 
        /// </summary>
        public const string DIRECTREPORTS = "directReports";

        /// <summary>
        /// Reprecenta la propiedad: name 
        /// Nombre completo
        /// </summary>
        public const string NAME = "name";

        /// <summary>
        /// Reprecenta la propiedad: objectGUID
        /// </summary>
        public const string OBJECTGUID = "objectGUID";

        /// <summary>
        /// Reprecenta la propiedad: userAccountControl Se utiliza para dehabilitar una cuenta.-
        /// Valor 514 dehabilita, mientras 512 mantiene la cuenta lista para logon.-
        /// </summary>
        public const string USERACCOUNTCONTROL = "userAccountControl";

        /// <summary>
        /// Reprecenta la propiedad: badPwdCount
        /// </summary>
        public const string BADPWDCOUNT = "badPwdCount";

        /// <summary>
        /// Reprecenta la propiedad: codePage
        /// </summary>
        public const string CODEPAGE = "codePage";
        /// <summary>
        /// Reprecenta la propiedad: countryCode
        /// </summary>
        public const string COUNTRYCODE = "countryCode";
        /// <summary>
        /// Reprecenta la propiedad: badPasswordTime
        /// </summary>
        public const string BADPASSWORDTIME = "badPasswordTime";
        /// <summary>
        /// Reprecenta la propiedad: lastLogoff
        /// </summary>
        public const string LASTLOGOFF = "lastLogoff";

        /// <summary>
        /// Reprecenta la propiedad: lastLogon
        /// </summary>
        public const string LASTLOGON = "lastLogon";

        /// <summary>
        /// Reprecenta la propiedad: pwdLastSet
        /// </summary>
        public const string PWDLASTSET = "pwdLastSet";

        /// <summary>
        /// Reprecenta la propiedad: primaryGroupID
        /// </summary>
        public const string PRIMARYGROUPID = "primaryGroupID";

        /// <summary>
        /// Reprecenta la propiedad: objectSid
        /// </summary>
        public const string OBJECTSID = "objectSid";

        /// <summary>
        /// Reprecenta la propiedad: adminCount
        /// </summary>
        public const string ADMINCOUNT = "adminCount";

        /// <summary>
        /// Reprecenta la propiedad: accountExpires
        /// </summary>
        public const string ACCOUNTEXPIRES = "accountExpires";

        /// <summary>
        /// Reprecenta la propiedad: logonCount
        /// </summary>
        public const string LOGONCOUNT = "logonCount";

        /// <summary>
        /// Reprecenta la propiedad: sAMAccountName
        /// </summary>
        public const string LOGINNAME = "sAMAccountName";

        /// <summary>
        /// Reprecenta la propiedad: sAMAccountType
        /// </summary>
        public const string SAMACCOUNTTYPE = "sAMAccountType";
        /// <summary>
        /// 
        /// </summary>
        public const string SHOWINADDRESSBOOK = "showInAddressBook";
        /// <summary>
        /// 
        /// </summary>
        public const string LEGACYEXCHANGEDN = "legacyExchangeDN";

        /// <summary>
        ///  Reprecenta la propiedad: userPrincipalName
        ///  Nombre usuario como jhendrix@actionline.com
        /// </summary>
        public const string USERPRINCIPALNAME = "userPrincipalName";
        /// <summary>
        /// 
        /// </summary>
        public const string EXTENSION = "ipPhone";
        /// <summary>
        /// 
        /// </summary>
        public const string SERVICEPRINCIPALNAME = "servicePrincipalName";

        /// <summary>
        /// Defines the Active Directory Schema category. For example, objectCategory = Person
        /// Ej: CN=Group,CN=Schema,CN=Configuration,DC=Datacom,DC=org
        /// </summary>
        public const string OBJECTCATEGORY = "objectCategory";
        /// <summary>
        /// 
        /// </summary>
        public const string DSCOREPROPAGATIONDATA = "dSCorePropagationData";
        /// <summary>
        /// 
        /// </summary>
        public const string LASTLOGONTIMESTAMP = "lastLogonTimestamp";
        /// <summary>
        /// 
        /// </summary>
        public const string EMAILADDRESS = "mail";
        /// <summary>
        /// 
        /// </summary>
        public const string MANAGER = "manager";
        /// <summary>
        /// 
        /// </summary>
        public const string MOBILE = "mobile";
        /// <summary>
        /// 
        /// </summary>
        public const string PAGER = "pager";
        /// <summary>
        /// 
        /// </summary>
        public const string FAX = "facsimileTelephoneNumber";
        /// <summary>
        /// 
        /// </summary>
        public const string HOMEPHONE = "homePhone";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHUSERACCOUNTCONTROL = "msExchUserAccountControl";
        /// <summary>
        /// 
        /// </summary>
        public const string MDBUSEDEFAULTS = "mDBUseDefaults";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHMAILBOXSECURITYDESCRIPTOR = "msExchMailboxSecurityDescriptor";
        /// <summary>
        /// 
        /// </summary>
        public const string HOMEMDB = "homeMDB";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHPOLICIESINCLUDED = "msExchPoliciesIncluded";
        /// <summary>
        /// 
        /// </summary>
        public const string HOMEMTA = "homeMTA";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHRECIPIENTTYPEDETAILS = "msExchRecipientTypeDetails";
        /// <summary>
        /// 
        /// </summary>
        public const string MAILNICKNAME = "mailNickname";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHHOMESERVERNAME = "msExchHomeServerName";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHVERSION = "msExchVersion";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHRECIPIENTDISPLAYTYPE = "msExchRecipientDisplayType";
        /// <summary>
        /// 
        /// </summary>
        public const string MSEXCHMAILBOXGUID = "msExchMailboxGuid";
        /// <summary>
        /// 
        /// </summary>
        public const string NTSECURITYDESCRIPTOR = "nTSecurityDescriptor";
        /// <summary>
        /// 
        /// </summary>
        public const string DESCRIPTION = "description";
        /// <summary>
        /// 
        /// </summary>
        public const string LOCKOUTTIME = "lockoutTime";
        /// <summary>
        /// 
        /// </summary>
        public const string ISACCOUNTLOCKED = "IsAccountLocked";

    }
}
