using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public partial class NavInfo
    {
        public IList<NavInfo> Children { get; set; }
    }
}
