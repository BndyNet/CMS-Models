using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

using Net.Bndy.Data;

namespace CMS.Repositories
{
    using CMS.Models;

    public class SinglePageRepository : _BaseRepository
    {
        public IList<SinglePage> GetPages(int? userId = null)
        {
            var models = new List<SinglePage>();
            if (userId.HasValue)
            {
                var rights = this.DbContext.UserRights
                    .Where(__ => __.UserId == userId && __.BOType == BusinessObjectType.SinglePage)
                    .Select(__ => __.BOId)
                    .ToList();
                models = this.DbContext.SinglePages
                    .Where(__ => rights.Contains(__.Id))
                    .OrderBy(__ => __.Id)
                    .ToList();
            }
            else
            {
                models = this.DbContext.SinglePages
                    .OrderBy(__ => __.Id)
                    .ToList();
            }
            foreach (var model in models)
            {
                model.AttachmentCount = this.DbContext.Attachments
                    .Count(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.SinglePage);
                model.CommentCount = this.DbContext.Comments
                    .Count(__ => __.BOId == model.Id && __.BOType == BusinessObjectType.SinglePage);
            }

            return models;
        }

        public SinglePage GetModel(int id)
        {
            var model = this.DbContext.SinglePages
                .FirstOrDefault(__ => __.Id == id);
            if (model != null)
            {
                model.Views += 1;
                this.DbContext.SaveChanges();

                model.Attachments = this.DbContext.Attachments
                    .Where(__ => __.BOId == id && __.BOType == BusinessObjectType.SinglePage)
                    .ToList();
            }

            return model;
        }

        public SinglePage Upsert(SinglePage model)
        {
            this.DbContext.Entry<SinglePage>(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
            this.DbContext.SaveChanges();

            return model;
        }
        public Page<Comment> GetComments(
            int singlePageId, int pageSize, int currentPage)
        {
            return GetPageListOrderBy<Comment, int>(pageSize, currentPage,
                (Comment __) => __.BOId == singlePageId && __.BOType == BusinessObjectType.SinglePage,
                (Comment __) => __.Id
            );
        }
        public void PostComment(int singlePageId, Comment comment)
        {
            comment.BOId = singlePageId;
            comment.BOType = BusinessObjectType.SinglePage;
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
    }
}
