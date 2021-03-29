

namespace pelsoft.auth.Amazon.be
{

    public class DomainUrlInfo
    {

        string domainName;

        /// <summary>
        /// Nombre de dominio
        /// </summary>
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }
        string lDAPPath;

        /// <summary>
        /// Url del dominio
        /// </summary>
        public string LDAPPath
        {
            get { return lDAPPath; }
            set { lDAPPath = value; }
        }
        string usr;

        /// <summary>
        /// Usuario de impersonalizacion
        /// </summary>
        public string Usr
        {
            get { return usr; }
            set { usr = value; }
        }
        string pwd;

        /// <summary>
        /// Password
        /// </summary>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        int _Id;
        /// <summary>
        /// 
        /// </summary>
        public string SiteName
        {
            get { return _SiteName; }
            set { _SiteName = value; }
        }
        string _SiteName;
        /// <summary>
        /// Distinguished Names A DN is a sequence of relative distinguished names (RDN) connected by commas.
        /// DC	domainComponent
        /// CN	commonName
        /// OU	organizationalUnitName
        /// O	organizationName
        /// etc
        /// </summary>
        public string DomainDN
        {
            get { return _DomainDN; }
            set { _DomainDN = value; }
        }
        string _DomainDN;
    }

}
