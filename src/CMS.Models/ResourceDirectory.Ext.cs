using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public partial class ResourceDirectory
    {
        [NotMapped]
        public List<ResourceDirectory> Children { get; set;}
        [NotMapped]
        public int Layer { get; set; }
        [NotMapped]
        public int FileCount { get; set; }
    }
}
