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


        private static List<NavInfo> _navs;
        public static List<NavInfo> Navs
        {
            get
            {
                if (_navs == null)
                {
                    _navs = new NavRepository().GetNavs();
                }
                return _navs;
            }
            set
            {
                _navs = value;
            }
        }

    }
}
