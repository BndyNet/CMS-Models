using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CMS.Repositories
{
    using CMS.Models;
    public class CommentRepository : _BaseRepository
    {
        public Comment Save(Comment model)
        {
            this.DbContext.Entry<Comment>(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
            this.DbContext.SaveChanges();
            return model;
        }

        public void Delete(int id)
        {
            var model = new Comment() { Id = id };
            this.DbContext.Comments.Attach(model);
            this.DbContext.Comments.Remove(model);
            this.DbContext.SaveChanges();
        }
    }
}
