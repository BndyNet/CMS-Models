using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public partial class Menu
    {
        public List<Menu> Children { get; set; }

        public Menu()
        {
            this.Children = new List<Menu>();
        }
    }
}
