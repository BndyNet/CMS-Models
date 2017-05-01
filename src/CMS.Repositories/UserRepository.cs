using Net.Bndy;
using Net.Bndy.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CMS.Repositories
{
    using Models;

    public class UserRepository : _BaseRepository
    {
        public User Get(string name, string pwd)
        {
            pwd = Cryptography.Hash(pwd, SeedData.HashSeed);
            return this.DbContext.Users
                .FirstOrDefault(__ => (
                    __.LoginName.Equals(name, StringComparison.OrdinalIgnoreCase)
                    ) && __.Password == pwd);
        }
        public List<User> GetList()
        {
            return this.DbContext.Users.ToList();
        }

        public User Get(int id)
        {
            return this.DbContext.Users
                .FirstOrDefault(__ => __.Id == id);
        }
        public void Delete(int id)
        {
            var u = new User() { Id = id };
            this.DbContext.Users.Attach(u);
            this.DbContext.Users.Remove(u);
            this.DbContext.SaveChanges();
        }
        public void ChangePassword(int userId, string newPwd)
        {
            var u = this.DbContext.Users.FirstOrDefault(__ => __.Id == userId);
            if (u != null)
            {
                u.Password = Cryptography.Hash(newPwd, SeedData.HashSeed);
                this.DbContext.SaveChanges();
            }
        }
        public Page<User> GetPageList(int pageSize, int currentPage, string keywords = null)
        {
            Page<User> result = null;
            if (string.IsNullOrWhiteSpace(keywords))
            {
                result = GetPageListOrderBy<User, int>(pageSize, currentPage,
                      (User __) => __.Role != UserRole.SuperAdministrator,
                      (User __) => __.Id
                );
            }
            else
            {
                result = GetPageListOrderBy<User, int>(pageSize, currentPage,
                   (User __) => (!string.IsNullOrWhiteSpace(__.LoginName) && __.LoginName.Contains(keywords)
                       || !string.IsNullOrWhiteSpace(__.RealName) && __.RealName.Contains(keywords))
                       && __.Role != UserRole.SuperAdministrator,
                   (User __) => __.Id
                );
            }

            foreach (var model in result.Data)
            {
                model.Groups = this.DbContext.UserGroups
                    .Where(__ => model.GroupIds != null && model.GroupIds.Contains("|" + __.Id + "|"))
                    .ToList();

                model.LoginHistories = GetTopNLoginHistories(model.Id, 5);
            }

            return result;
        }

        public User Save(User user)
        {
            var u = this.DbContext.Users.FirstOrDefault(__ => __.Id == user.Id);
            if (u != null)
            {
                u.Enabled = user.Enabled;
                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    u.Password = Cryptography.Hash(user.Password, SeedData.HashSeed);
                }
            }
            else
            {
                user.Role = UserRole.Employee;
                user.Password = Cryptography.Hash(user.Password, SeedData.HashSeed);
                this.DbContext.Users.Add(user);
            }

            this.DbContext.SaveChanges();

            return user;
        }

        public void LoginHistory(User user, HttpRequestBase request)
        {
            this.DbContext.UserLoginHistories.Add(new UserLoginHistory()
            {
                ClientIP = request.UserHostAddress,
                Browser = string.Format("{0} {1}", request.Browser.Browser, request.Browser.Version),
                UserId = user.Id,
                LoggedOn = DateTime.Now,
            });
            this.DbContext.SaveChanges();
        }
        public List<UserLoginHistory> GetTopNLoginHistories(int userId, int top)
        {
            return this.DbContext.UserLoginHistories
                .Where(__ => __.UserId == userId)
                .OrderByDescending(__ => __.LoggedOn)
                .Take(top)
                .ToList();
        }

        public List<UserRights> GetUserRights(int userId)
        {
            return this.DbContext.UserRights.Where(__ => __.UserId == userId)
                .ToList();
        }
        public void SetRights(int userId, List<UserRights> rights)
        {
            this.DbContext.UserRights.RemoveRange(
                this.DbContext.UserRights.Where(__ => __.UserId == userId));
            this.DbContext.SaveChanges();

            this.DbContext.UserRights.AddRange(rights);
            this.DbContext.SaveChanges();
        }
    }
}
