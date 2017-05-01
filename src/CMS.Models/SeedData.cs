using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    using Net.Bndy;

    public class SeedData
    {
        public static string HashSeed = "admin";

        public static User SupperAdmin = new User
        {
            LoginName = "admin",
            Password = Cryptography.Hash("admin123456", HashSeed),
            Role = UserRole.SuperAdministrator,
            Enabled = true,
        };

        public static List<Menu> Menus = new List<Menu>()
        {
            new Menu { Text = "Menu 1", ContentType = ContentType.Article, Category = MenuCategory.Main,
                Children = new List<Menu> {
                    new Menu { Text = "Menu 1-1", ContentType = ContentType.Article, Category = MenuCategory.Main },
                    new Menu { Text = "Menu 1-2", ContentType = ContentType.Resource, Category = MenuCategory.Main },
                    new Menu { Text = "Menu 1-3", ContentType = ContentType.SinglePage, Category = MenuCategory.Main,
                        Children =new List<Menu> {
                            new Menu { Text = "Menu 1-3-1", ContentType = ContentType.Article, Category = MenuCategory.Main },
                            new Menu { Text = "Menu 1-3-2", ContentType = ContentType.Resource, Category = MenuCategory.Main },
                            new Menu { Text = "Menu 1-3-3", ContentType = ContentType.SinglePage, Category = MenuCategory.Main },
                            new Menu { Text = "Menu 1-3-4", ContentType = ContentType.Article, Category = MenuCategory.Main },
                        } },
                    new Menu { Text = "Menu 1-4", ContentType = ContentType.Article, Category = MenuCategory.Main },
                }
            },
            new Menu { Text = "Menu 2", ContentType = ContentType.Link, Category = MenuCategory.Main },
            new Menu { Text = "Menu 3", ContentType = ContentType.Resource, Category = MenuCategory.Main },
            new Menu { Text = "Menu 4", ContentType = ContentType.SinglePage, Category = MenuCategory.Main },
            new Menu { Text = "Menu 5", ContentType = ContentType.Article, Category = MenuCategory.Main },
        };
    }
}
