using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    using CMS.Models;
    public class UserGroupRepository : _BaseRepository
    {
        public List<UserGroup> Models { get; private set; }
        public UserGroupRepository()
        {
            Models = this.DbContext.UserGroups.ToList();
        }

        public UserGroup Get(int id)
        {
            return this.Models.FirstOrDefault(__ => __.Id == id);
        }
        public List<UserGroup> GetList()
        {
            return this.Models.ToList();
        }
    }
}
