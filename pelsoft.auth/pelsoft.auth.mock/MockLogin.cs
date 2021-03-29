using System.Collections.Generic;

namespace pelsoft.auth.helpers
{
    public class MockLogins : List<MockLogin>
    {

    }
    public class MockLogin
    {
        public string provider { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
    }
}

