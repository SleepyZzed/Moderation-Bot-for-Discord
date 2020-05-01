using System;
using System.Collections.Generic;
using System.Text;

namespace Cerberus.UserProfiles
{
    public class UserAccounts
    {
        public ulong ID { get; set; }

        public uint Warns { get; set; }

        public string Roles { get; set; }

        public string warnreason { get; set; }

        public int Mutes { get; set; }

        public string Username { get; set; }
        public List<string> ListofRoles { get; set; }
        public List<string> WarnreasonList { get; set; }

        public bool Muted { get; set; }
    }
}
