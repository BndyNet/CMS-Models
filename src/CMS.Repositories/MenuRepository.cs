using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    using CMS.Models;
    public class MenuRepository : _BaseRepository
    {
        public Menu Get(int? id)
        {
            if (!id.HasValue)
                return null;

            return this.DbContext.Menus
                .FirstOrDefault(__ => __.Id == id);
        }

        public Menu Get(Func<Menu, bool> predicate)
        {
            return this.DbContext.Menus
                .Where(predicate)
                .FirstOrDefault();
        }

        public List<Menu> GetMenus(MenuCategory? category = null)
        {
            var menus = this.DbContext.Menus.Where(__ => ((category.HasValue && __.Category == category) || !category.HasValue) && __.IsHidden != true).ToList();

            var rootMenus = menus.Where(__ => !__.ParentId.HasValue)
                .OrderBy(__ => __.DisplayOrder)
                .ToList();

            foreach (var menu in rootMenus)
            {
                GetChildren(menu, menus);
            }

            return rootMenus;
        }
        private void GetChildren(Menu nav, List<Menu> source)
        {
            nav.Children = source.Where(__ => __.ParentId == nav.Id).ToList();
            foreach (var n in nav.Children)
            {
                GetChildren(n, source);
            }
        }
        public List<Menu> GetChildren(int menuId)
        {
            return this.DbContext.Menus.Where(__ => __.ParentId == menuId)
                .ToList();
        }
    }
}
