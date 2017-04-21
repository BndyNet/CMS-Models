using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public partial class Article
    {
        public List<Attachment> Attachments = new List<Attachment>();
        public List<ArticleAttributeValue> CustomAttributes = new List<ArticleAttributeValue>();
        public int AttachmentCount { get; set; }
        public int CommentCount { get; set; }
        public List<User> AllUsers { get; set; }

        public User User { get; set; }
             
        public string Summary
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Content))
                {
                    var s = Regex.Replace(this.Content, @"</?\w+.*?>", "");
                    if (s.Length > 200)
                    {
                        return s.Substring(0, 200) + "...";
                    }
                    return s;
                }
                return "";
            }
        }
        public ArticleCategory Category { get; set; }
    }
}
