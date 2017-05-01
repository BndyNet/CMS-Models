using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    using CMS.Models;
    public class ResourceRepository : _BaseRepository
    {
        public List<Resource> GetList(int? userGroupId = null)
        {
            if (userGroupId.HasValue)
            {
                return this.DbContext.Resources.Where(__ => __.GroupIds != null && __.GroupIds.Contains("|" + userGroupId + "|"))
                    .OrderByDescending(__ => __.CreatedDate)
                    .ToList();
            }
            else
            {
                return this.DbContext.Resources.OrderByDescending(__ => __.CreatedDate).ToList();
            }
        }
        public int GetFileCountInDirectory(int dirId)
        {
            return this.DbContext.Resources.Count(__ => __.DirectoryId == dirId);
        }
        public int GetFileCountInUserGroup(int userGroupId)
        {
            return this.DbContext.Resources.Count(__ => __.GroupIds != null && __.GroupIds.Contains("|" + userGroupId + "|"));
        }
        public List<ResourceDirectory> GetDirs(int? userGroupId)
        {
            var result = new List<ResourceDirectory>();
            var items = this.DbContext.ResourceDirectories.Where(__ => __.UserGroupId == userGroupId).ToList();
            Action<IEnumerable<ResourceDirectory>, int> setLayers = null;
            setLayers = (dirs, layer) =>
            {
                foreach (var item in dirs)
                {
                    item.Layer = layer;
                    result.Add(item);
                    setLayers(items.Where(c => c.ParentDirectoryId == item.Id).ToList(), layer + 1);
                }

            };
            setLayers(items.Where(___ => ___.ParentDirectoryId == null || ___.ParentDirectoryId.Value <= 0), 0);

            return result;
        }
        public List<ResourceDirectory> GetChildDirs(int dirId)
        {
            var lst = this.DbContext.ResourceDirectories.Where(__ => __.ParentDirectoryId == dirId).ToList();
            foreach (var child in lst)
            {
                child.Children = GetDirs(child.Id);
            }
            return lst;
        }
        public ResourceDirectory GetDirectoryWithChildren(int dirId)
        {
            var m = this.DbContext.ResourceDirectories.FirstOrDefault(__ => __.Id == dirId);
            if (m != null)
            {
                m.Children = GetChildDirs(m.Id);

            }
            return m;
        }
        public ResourceDirectory UpsertDir(ResourceDirectory dir)
        {
            if (dir.Id > 0)
            {
                var d = this.DbContext.ResourceDirectories.FirstOrDefault(__ => __.Id == dir.Id);
                if (d != null)
                {
                    d.ParentDirectoryId = dir.ParentDirectoryId;
                    d.DirectoryName = dir.DirectoryName;
                    d.ParentDirectoryId = dir.ParentDirectoryId;
                    dir = d;
                }
            }
            else
            {
                this.DbContext.ResourceDirectories.Add(dir);
            }
            this.DbContext.SaveChanges();

            return dir;

        }
        public bool DeleteDir(int id)
        {
            var d = this.DbContext.ResourceDirectories.FirstOrDefault(__ => __.Id == id);

            if (d != null && !this.DbContext.ResourceDirectories.Any(__ => __.ParentDirectoryId == d.Id)
                && !this.DbContext.Resources.Any(__ => __.DirectoryId == d.Id))
            {
                this.DbContext.ResourceDirectories.Remove(d);
                this.DbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Add(Resource r)
        {
            this.DbContext.Resources.Add(r);
            this.DbContext.SaveChanges();
        }
        public void Update(Resource r)
        {
            var m = this.DbContext.Resources.FirstOrDefault(__ => __.Id == r.Id);
            if (m != null)
            {
                m.Description = r.Description;
                m.FileExtensionName = r.FileExtensionName;
                m.FileName = r.FileName;
                m.FilePath = r.FilePath;
                this.DbContext.SaveChanges();
            }
        }
        public Resource Get(int id)
        {
            return this.DbContext.Resources.FirstOrDefault(__ => __.Id == id);
        }

        public void Delete(int id, Action<Resource> before)
        {
            var model = this.DbContext.Resources.FirstOrDefault(__ => __.Id == id);
            if (model != null)
            {
                if (before != null)
                {
                    before(model);
                }

                this.DbContext.Resources.Remove(model);
                this.DbContext.SaveChanges();
            }
        }
    }
}
