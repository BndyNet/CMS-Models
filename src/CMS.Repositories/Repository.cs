using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    using CMS.Models;
    public static class Repository
    {
        private static List<Link> _links;
        public static List<Link> Links
        {
            get
            {
                if (_links == null)
                {
                    _links = new LinkRepository().GetList();
                }
                return _links;
            }
            set
            {
                _links = value;
            }
        }


        private static List<Menu> _menus;
        public static List<Menu> Menus
        {
            get
            {
                if (_menus == null)
                {
                    _menus = new MenuRepository().GetMenus();
                }
                return _menus;
            }
            set
            {
                _menus = value;
            }
        }

    }
}
