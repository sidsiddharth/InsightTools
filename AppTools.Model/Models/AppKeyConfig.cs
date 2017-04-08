using System;
using System.Collections.Generic;
using System.Text;

namespace AppTools.Model
{
    public class AppKeyConfig
    {
        public string AccessKey { get; set; } = string.Empty;
        public string AccessSecret { get; set; } = string.Empty;
        public string LDAPServer { get; set; } = string.Empty;
        public Int16 LDAPServerPort { get; set; } = 0;
        public string LDAPUser { get; set; } = string.Empty;
        public string LDAPUserPass { get; set; } = string.Empty;

    }
}
