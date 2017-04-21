using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public partial class User
    {
        public List<UserGroup> Groups { get; set; }
        public List<UserLoginHistory> LoginHistories { get; set; }
        public List<UserRights> Rights { get; set; }
    }
}
