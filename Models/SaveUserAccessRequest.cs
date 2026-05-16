
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace fintech.Models
{
    public class SaveUserAccessRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<UserModuleAccessItem> ModuleAccess { get; set; }
    }

    public class UserModuleAccessItem
    {
        public string ModuleDesc { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
    }

}