using System;
using System.Collections.Generic;
using System.Text;

namespace microsoft.keppcont.api.common.BE
{
    public class Requets
    {
        public Guid UserId { get; set; }
        public String ClientId { get; set; }

        public string SecurityProviderName { get; set; }
    }
}
