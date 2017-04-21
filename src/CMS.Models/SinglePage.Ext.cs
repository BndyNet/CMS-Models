using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public partial class SinglePage
    {
        public List<Attachment> Attachments = new List<Attachment>();
        public int AttachmentCount { get; set; }
        public int CommentCount { get; set; }
    }
}
