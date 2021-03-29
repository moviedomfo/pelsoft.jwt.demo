using System;

namespace pelsoft.auth.Amazon.ActiveDirectory
{
    public class DomainController
    {
        int _Id;
        int _DomainId;
        string _HostName;
        string _Ip;

        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
        public int DomainId
        {
            get
            {
                return _DomainId;
            }
            set
            {
                _DomainId = value;
            }
        }
        public string HostName
        {
            get
            {
                return _HostName;
            }
            set
            {
                _HostName = value;
            }
        }
        public string Ip
        {
            get
            {
                return _Ip;
            }
            set
            {
                _Ip = value;
            }
        }

    }
}
