using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    using CMS.Models;
    public class NavRepository : _BaseRepository
    {
        public NavInfo Get(int? id)
        {
            if (!id.HasValue)
                return null;

            return this.DbContext.NavInfoes
                .FirstOrDefault(__ => __.Id == id);
        }

        public NavInfo Get(Func<NavInfo, bool> predicate)
        {
            return this.DbContext.NavInfoes
                .Where(predicate)
                .FirstOrDefault();
        }

        public List<NavInfo> GetNavs(NavType? type = null)
        {
            var navs = this.DbContext.NavInfoes.Where(__ => ((type.HasValue && __.NavType == type) || !type.HasValue) && __.IsHidden != true).ToList();

            var rootNav = navs.Where(__ => !__.ParentNavId.HasValue).OrderBy(__ => __.DisplayOrder).ToList();

            foreach (var nav in rootNav)
            {
                GetChildren(nav, navs);
            }

            return rootNav;
        }
        private void GetChildren(NavInfo nav, List<NavInfo> source)
        {
            nav.Children = source.Where(__ => __.ParentNavId == nav.Id).ToList();
            foreach (var n in nav.Children)
            {
                GetChildren(n, source);
            }
        }
        public List<NavInfo> GetChildren(int navId)
        {
            return this.DbContext.NavInfoes.Where(__ => __.ParentNavId == navId)
                .ToList();
        }
    }
}
