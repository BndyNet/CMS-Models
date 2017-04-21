using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Net.Bndy.IO;

namespace CMS.Repositories
{
    using CMS.Models;

    public class AttachementRepository : _BaseRepository
    {
        public List<Attachment> GetAttachemnts(int boid, BusinessObjectType boType)
        {
            return this.DbContext.Attachments
                   .Where(__ => __.BOId == boid && __.BOType == boType)
                   .OrderByDescending(__ => __.IsImage)
                   .ToList();
        }
        public List<Attachment> GetImageAttachemnts(int boid, BusinessObjectType boType)
        {
            return this.DbContext.Attachments
               .Where(__ => __.BOId == boid && __.BOType == boType && FileHelper.ImageExtensionNames.Contains(__.FileExtensionName.ToLower()))
               .ToList();
        }
        public Attachment Add(Attachment model)
        {
            this.DbContext.Attachments.Add(model);
            this.DbContext.SaveChanges();
            return model;
        }
        public void Delete(int id)
        {
            var model = this.DbContext.Attachments.FirstOrDefault(__ => __.Id == id);
            if (model != null)
            {
                this.DbContext.Entry<Attachment>(model).State = System.Data.Entity.EntityState.Deleted;
                this.DbContext.SaveChanges();

                var file = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, model.FilePath);
                if (File.Exists(file) && model.FilePath.StartsWith("/upload/"))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
