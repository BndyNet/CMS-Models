using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CMS.Repositories
{
    using CMS.Models;
    public class LinkRepository : _BaseRepository
    {
        public Link Get(int id)
        {
            return this.DbContext.Links.FirstOrDefault(__ => __.Id == id);
        }
        public List<Link> GetList()
        {
            return this.DbContext.Links.OrderBy(__ => __.DisplayOrder).ToList();
        }

        public Link Save(Link model)
        {
            this.DbContext.Entry<Link>(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
            this.DbContext.SaveChanges();

            Repository.Links = null;

            return model;
        }

        public void Delete(int id)
        {
            var l = new Link() { Id = id };
            this.DbContext.Links.Attach(l);
            this.DbContext.Links.Remove(l);
            this.DbContext.SaveChanges();

            Repository.Links = null;
        }
        public void Delete(Link model)
        {
            this.DbContext.Links.Remove(model);
            this.DbContext.SaveChanges();

            Repository.Links = null;
        }
    }
}
