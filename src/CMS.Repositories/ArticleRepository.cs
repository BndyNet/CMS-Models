using Net.Bndy.Data;
using Net.Bndy.IO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.Repositories
{
    using Models;

    public class ArticleRepository : _BaseRepository
    {
        public Article Save(Article entity)
        {
            this.DbContext.Entry<Article>(entity).State = entity.Id > 0
                ? EntityState.Modified : EntityState.Added;
            if (entity.Id > 0)
            {
                entity.ModifiedDate = DateTime.Now;
            }
            else
            {
                entity.CreatedDate = DateTime.Now;
                entity.ModifiedDate = entity.CreatedDate;
            }
            this.DbContext.SaveChanges();

            this.DbContext.ArticleAttributeValues.RemoveRange(
                this.DbContext.ArticleAttributeValues.Where(__ => __.ArticleId == entity.Id)
                );
            foreach (var attr in entity.CustomAttributes)
            {
                this.DbContext.ArticleAttributeValues.Add(new ArticleAttributeValue()
                {
                    ArticleId = entity.Id,
                    AttrId = attr.AttrId,
                    AttrValue = attr.AttrValue,
                });
            }
            this.DbContext.SaveChanges();

            return entity;
        }

        public void Delete(int id)
        {
            var model = new Article() { Id = id };
            this.DbContext.Articles.Attach(model);
            this.DbContext.Articles.Remove(model);
            this.DbContext.SaveChanges();
        }

        public ArticleCategory GetCategory(int id)
        {
            return this.DbContext.ArticleCategories
                .FirstOrDefault(__ => __.Id == id);
        }
        public ArticleCategory GetCategoryByName(string name)
        {
            return this.DbContext.ArticleCategories
                .FirstOrDefault(__ => __.Name == name);
        }
        public List<ArticleCategory> GetCategories()
        {
            return this.DbContext.ArticleCategories
                .OrderBy(__ => __.DisplayOrder)
                .ToList();
        }
        public List<ArticleCategory> GetCategories(int userId)
        {
            var rights = this.DbContext.UserRights
                .Where(__ => __.UserId == userId && __.BOType == BusinessObjectType.Article)
                .Select(__ => __.BOId)
                .ToList();

            return this.DbContext.ArticleCategories
                .Where(__ => rights.Contains(__.Id))
                .OrderBy(__ => __.DisplayOrder)
                .ToList();
        }
        public List<ArticleCategory> GetCategories(string groupOrCategory)
        {
            return this.DbContext.ArticleCategories
                .Where(__ => __.Name == groupOrCategory)
                .OrderBy(__ => __.DisplayOrder)
                .ToList();
        }
        public Article GetModel(int id)
        {
            var model = this.DbContext.Articles
                .FirstOrDefault(__ => __.Id == id);
            if (model != null)
            {
                model.Views += 1;
                this.DbContext.SaveChanges();

                model.Attachments = this.DbContext.Attachments
                    .Where(__ => __.BOId == id && __.BOType == BusinessObjectType.Article)
                    .ToList();
                model.AttachmentCount = model.Attachments.Count;
                model.CommentCount = this.DbContext.Comments
                    .Count(__ => __.BOId == id && __.BOType == BusinessObjectType.Article);
                model.Category = this.GetCategory(model.CategoryId);
            }

            return model;
        }

        public List<Article> GetTopModels(int top, int categoryId)
        {
            var models = this.DbContext.Articles
                .Where(__ => __.CategoryId == categoryId)
                .OrderByDescending(__ => __.ModifiedDate)
                .Take(top)
                .ToList();
            foreach (var model in models)
            {
                model.Attachments = this.DbContext.Attachments
                    .Where(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article)
                    .ToList();

                model.AttachmentCount = model.Attachments.Count;

                model.CommentCount = this.DbContext.Comments
                    .Count(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article);

                model.Category = this.GetCategory(model.CategoryId);
            }

            return models;
        }
        public List<Article> GetTopModelsWithImages(int top, string categoryName)
        {
            var result = new List<Article>();
            var category = this.DbContext.ArticleCategories.FirstOrDefault(__ => __.Name == categoryName);

            if (category != null)
            {
                var models = this.DbContext.Articles
                        .Where(__ => __.CategoryId == category.Id && __.HasImageAttachments)
                        .OrderByDescending(__ => __.ModifiedDate)
                        .Take(top)
                        .ToList();

                foreach (var model in models)
                {
                    model.Attachments = this.DbContext.Attachments
                        .Where(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article && FileHelper.ImageExtensionNames.Contains(__.FileExtensionName))
                        .ToList();

                    model.Category = this.GetCategory(model.CategoryId);
                }

                result = models;
            }

            return result;
        }
        public List<Article> GetTopModels(int top, string categoryName, out ArticleCategory category)
        {
            var result = new List<Article>();
            category = this.DbContext.ArticleCategories.FirstOrDefault(__ => __.Name == categoryName);

            if (category != null)
            {
                var categoryId = category.Id;
                result = this.DbContext.Articles
                    .Where(__ => __.CategoryId == categoryId)
                    .OrderByDescending(__ => __.ModifiedDate)
                    .Take(top)
                    .ToList();
            }
            else
            {
                var categoryIds = this.DbContext.ArticleCategories
                    .Where(__ => __.Name == categoryName)
                    .Select(__ => __.Id)
                    .ToList();

                result = this.DbContext.Articles
                    .Where(__ => categoryIds.Contains(__.CategoryId))
                    .OrderByDescending(__ => __.ModifiedDate)
                    .Take(top)
                    .ToList();
            }

            foreach (var model in result)
            {
                //model.Attachments = this.DbContext.Attachments
                //    .Where(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article)
                //    .ToList();

                //model.AttachmentCount = model.Attachments.Count;

                //model.CommentCount = this.DbContext.Comments
                //    .Count(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article);

                model.Category = this.GetCategory(model.CategoryId);
            }

            return result;
        }
        public Page<Article> GetPageList(int categoryId,
            int pageSize, int currentPage, string keywords = null)
        {
            Page<Article> result = null;
            if (string.IsNullOrWhiteSpace(keywords))
            {
                result = GetPageListOrderByDescending<Article, DateTime>(pageSize, currentPage,
                      (Article __) => __.CategoryId == categoryId,
                      (Article __) => __.ModifiedDate
                );
            }
            else
            {
                result = GetPageListOrderByDescending<Article, DateTime>(pageSize, currentPage,
                   (Article __) => __.CategoryId == categoryId && __.Title.Contains(keywords),
                   (Article __) => __.ModifiedDate
                );
            }

            foreach (var model in result.Data)
            {
                model.AttachmentCount = this.DbContext.Attachments
                    .Count(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article);
                model.CommentCount = this.DbContext.Comments
                    .Count(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.Article);
            }

            return result;
        }

        public Page<Comment> GetComments(
            int articleId, int pageSize, int currentPage)
        {
            return GetPageListOrderBy<Comment, int>(pageSize, currentPage,
                (Comment __) => __.BOId == articleId && __.BOType == BusinessObjectType.Article,
                (Comment __) => __.Id
            );
        }

        public void PostComment(int articleId, Comment comment)
        {
            comment.BOId = articleId;
            comment.BOType = BusinessObjectType.Article;
            this.DbContext.Comments.Add(comment);
            this.DbContext.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var model = new Comment() { Id = id };
            this.DbContext.Comments.Attach(model);
            this.DbContext.Comments.Remove(model);
            this.DbContext.SaveChanges();
        }


        public List<ArticleAttributeDefinition> GetAttributeDefinitions(int articleCategoryId)
        {
            return this.DbContext.ArticleAttributeDefinitions
                .Where(__ => __.ArticleCategoryId == articleCategoryId)
                .OrderBy(__ => __.DisplayOrder)
                .ToList();
        }

        public List<ArticleAttributeValue> GetEmptyAttributeValues(int categoryId)
        {
            var result = new List<ArticleAttributeValue>();

            foreach (var item in GetAttributeDefinitions(categoryId))
            {
                result.Add(new ArticleAttributeValue()
                {
                    AttrValue = item.InitialValue,
                    AttrId = item.Id,
                    ArticleAttributeDefinition = item,
                });
            }

            return result;
        }
        public List<ArticleAttributeValue> GetAttributeValues(int articleId)
        {
            var article = GetModel(articleId);
            var result = this.DbContext.ArticleAttributeValues
                .Where(__ => __.ArticleId == articleId)
                .ToList();

            foreach (var attrDef in GetAttributeDefinitions(article.CategoryId))
            {
                if (!result.Any(__ => __.AttrId == attrDef.Id))
                {
                    result.Add(new ArticleAttributeValue()
                    {
                        ArticleId = article.Id,
                        AttrValue = attrDef.InitialValue,
                        ArticleAttributeDefinition = attrDef,
                        AttrId = attrDef.Id,
                    });
                }
            }

            return result;
        }
    }
}
