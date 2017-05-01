using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CMS.Models
{
    using Net.Bndy;
    using Net.Bndy.IO;

    public class DbInitializer
    {
        static DbInitializer()
        {
        }

        public static void InitMenus(List<Menu> menus, ModelContainer container)
        {
            for (var idx = 0; idx < menus.Count; idx++)
            {
                var item = menus[idx];
                item = container.Menus.Add(item);
                container.SaveChanges();

                foreach (var child in item.Children)
                {
                    child.ParentId = item.Id;
                }

                InitMenus(item.Children, container);
            }
        }

        public static void Init()
        {
            using (var container = new ModelContainer())
            {
                if (container.Database.Exists())
                    return;

                container.Database.CreateIfNotExists();

                #region Administrator
                container.Users.Add(SeedData.SupperAdmin);
                container.SaveChanges();
                #endregion Administrator

                #region Initialize Nav Contents
                InitMenus(SeedData.Menus, container);

                var dataDirectoryName = "App_Data\\Seed";
                var menus = container.Menus
                    .Where(__ => __.ContentType == ContentType.Article || __.ContentType == ContentType.SinglePage)
                    .ToList();
                foreach (var menu in menus)
                {
                    Menu parent = null;
                    if (menu.ParentId.HasValue)
                    {
                        parent = container.Menus.FirstOrDefault(__ => __.Id == menu.ParentId);
                    }
                    switch (menu.ContentType)
                    {
                        case ContentType.Article:
                            var ac = new ArticleCategory();
                            ac.Name = menu.Text;
                            ac.EnableAttachment = true;
                            ac.EnableComment = true;

                            ac = container.ArticleCategories.Add(ac);
                            container.SaveChanges();

                            // Initialize Data
                            var adir = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                dataDirectoryName,
                                ac.Name
                                );
                            if (Directory.Exists(adir))
                            {
                                foreach (var item in new DirectoryInfo(adir).GetDirectories())
                                {
                                    var article = new Article();
                                    article.Title = item.Name;
                                    article.CategoryId = ac.Id;
                                    article.CreatedDate = DateTime.Now;
                                    article.ModifiedDate = DateTime.Now;
                                    article.EnableAttachment = ac.EnableAttachment;
                                    article.EnableComment = ac.EnableComment;

                                    var contentFile = Path.Combine(item.FullName, "content.html");
                                    if (File.Exists(contentFile))
                                    {
                                        article.Content = File.ReadAllText(contentFile);
                                    }

                                    article = container.Articles.Add(article);
                                    container.SaveChanges();
                                    foreach (var attachment in item.GetFiles())
                                    {
                                        if (!attachment.Name.Equals("content.html", StringComparison.OrdinalIgnoreCase))
                                        {
                                            var atta = new Attachment();
                                            atta.BOId = article.Id;
                                            atta.BOType = BusinessObjectType.Article;
                                            atta.FileExtensionName = attachment.Extension;
                                            atta.FileName = attachment.Name;
                                            atta.FilePath = "/" + string.Format(Path.Combine(adir, item.Name, attachment.Name)
                                                .Replace(AppDomain.CurrentDomain.BaseDirectory, ""))
                                                .Replace("\\", "/");
                                            atta.UploadedDate = DateTime.Now;
                                            if (FileHelper.ImageExtensionNames.Contains(atta.FileExtensionName))
                                            {
                                                article.HasImageAttachments = true;
                                                atta.IsImage = true;
                                            }

                                            container.Attachments.Add(atta);
                                        }
                                    }
                                    container.SaveChanges();
                                }
                            }
                            else
                            {
                                for (var i = 0; i < 23; i++)
                                {
                                    container.Articles.Add(new Article()
                                    {
                                        Title = ac.Name + i.ToString(),
                                        Content = ac.Name + i.ToString(),
                                        CategoryId = ac.Id,
                                        CreatedDate = DateTime.Now,
                                        ModifiedDate = DateTime.Now,
                                        EnableAttachment = ac.EnableAttachment,
                                        EnableComment = ac.EnableComment,
                                    });
                                }
                                container.SaveChanges();
                            }

                            menu.Uri = string.Format("/article/list/{0}", ac.Id);
                            break;

                        case ContentType.SinglePage:
                            var sp = new SinglePage();
                            sp.Title = menu.Text;

                            sp.EnableAttachment = false;
                            sp.EnableComment = false;
                            sp = container.SinglePages.Add(sp);
                            container.SaveChanges();

                            var sdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                dataDirectoryName,
                                sp.Title);
                            if (Directory.Exists(sdir))
                            {
                                if (File.Exists(Path.Combine(sdir, "content.html")))
                                {
                                    sp.Content = File.ReadAllText(Path.Combine(sdir, "content.html"));
                                }
                                foreach (var attachment in new DirectoryInfo(sdir).GetFiles())
                                {
                                    if (!attachment.Name.Equals("content.html", StringComparison.OrdinalIgnoreCase))
                                    {
                                        var atta = new Attachment();
                                        atta.BOId = sp.Id;
                                        atta.BOType = BusinessObjectType.Article;
                                        atta.FileExtensionName = attachment.Extension;
                                        atta.FileName = attachment.Name;
                                        atta.FilePath = "/" + string.Format(Path.Combine(sdir, sp.Title, attachment.Name)
                                            .Replace(AppDomain.CurrentDomain.BaseDirectory, ""))
                                            .Replace("\\", "/");
                                        atta.UploadedDate = DateTime.Now;
                                        if (FileHelper.ImageExtensionNames.Contains(atta.FileExtensionName))
                                        {
                                            sp.HasImageAttachments = true;
                                            atta.IsImage = true;
                                        }
                                        container.Attachments.Add(atta);
                                    }
                                }
                                container.SaveChanges();
                            }
                            menu.Uri = string.Format("/page/index/{0}", sp.Id);
                            break;
                    }
                }
                container.SaveChanges();

                #endregion Initialize Nav Contents
            }
        }

        public static ModelContainer GetDbContext(string name = null)
        {
            return new ModelContainer();
        }
    }
}
