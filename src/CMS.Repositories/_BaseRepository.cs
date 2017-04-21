using Net.Bndy.Data;
using System;
using System.Data.Entity;
using System.Linq;

namespace CMS.Repositories
{
    using CMS.Models;

    public abstract class _BaseRepository
    {
        public static string DbNameOrConnectionString { get; set; }
        public ModelContainer DbContext { get; set; }

        public _BaseRepository()
        {
            this.DbContext = DbInitializer.GetDbContext(DbNameOrConnectionString);
        }

        public Page<TEntity> GetPageListOrderBy<TEntity, TOrderByKey>(int pageSize, int currentPage,
            Func<TEntity, bool> predicate, Func<TEntity, TOrderByKey> orderBy)
            where TEntity : class
        {
            var result = new Page<TEntity>(pageSize, currentPage);
            result.RecordCount = this.DbContext.Set<TEntity>().Count(predicate);
            if (currentPage > result.PageCount)
            {
                currentPage = result.PageCount;
            }
            if (pageSize > 0)
            {
                result.Data = this.DbContext.Set<TEntity>()
                    .Where(predicate)
                    .OrderBy(orderBy)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.Data = this.DbContext.Set<TEntity>()
                    .Where(predicate)
                    .OrderBy(orderBy)
                    .ToList();
            }

            return result;
        }
        public Page<TEntity> GetPageListOrderByDescending<TEntity, TOrderByKey>(int pageSize, int currentPage,
             Func<TEntity, bool> predicate, Func<TEntity, TOrderByKey> orderByDescending)
             where TEntity : class
        {
            var result = new Page<TEntity>(pageSize, currentPage);
            result.RecordCount = this.DbContext.Set<TEntity>().Count(predicate);
            if (currentPage > result.PageCount)
            {
                currentPage = result.PageCount;
            }
            if (pageSize > 0)
            {
                result.Data = this.DbContext.Set<TEntity>()
                    .Where(predicate)
                    .OrderByDescending(orderByDescending)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.Data = this.DbContext.Set<TEntity>()
                    .Where(predicate)
                    .OrderByDescending(orderByDescending)
                    .ToList();
            }

            return result;
        }

        public void LogException(string url, string message, string detail = null)
        {
            this.DbContext.AppExceptions.Add(new AppException()
            {
                Detail = detail,
                Message = message,
                OccuredDate = DateTime.Now,
                Url = url,
            });
            this.DbContext.SaveChanges();
        }
    }
}
