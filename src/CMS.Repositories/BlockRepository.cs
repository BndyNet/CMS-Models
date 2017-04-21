using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CMS.Repositories
{
    using CMS.Models;
    public class BlockRepository : _BaseRepository
    {
        public BlockInfo Get(string title)
        {
            return this.DbContext.BlockInfoes.FirstOrDefault(__ => __.Title == title);
        }
        public List<BlockInfo> GetList()
        {
            return this.DbContext.BlockInfoes.ToList();
        }

        public BlockInfo Save(BlockInfo block)
        {
            this.DbContext.Entry<BlockInfo>(block).State =
                block.Id > 0 ? EntityState.Modified : EntityState.Added;
            this.DbContext.SaveChanges();
            return block;
        }

        public void Delete(int id)
        {
            var model = new BlockInfo() { Id = id };
            this.DbContext.BlockInfoes.Attach(model);
            this.DbContext.BlockInfoes.Remove(model);
            this.DbContext.SaveChanges();
        }
    }
}
