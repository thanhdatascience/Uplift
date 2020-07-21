using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int Id)
        {
            return dbSet.Find(Id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> querry = dbSet;
            if (filter != null)
            {
                querry = querry.Where(filter);
            }
            //include properties will be comma seperated
            if (includeProperties != null)
            {
                foreach(var includeProperty in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    querry = querry.Include(includeProperty);
                }
            }

            if(orderBy != null)
            {
                return orderBy(querry).ToList();
            }
            return querry.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> querry = dbSet;
            if (filter != null)
            {
                querry = querry.Where(filter);
            }
            //include properties will be comma seperated
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    querry = querry.Include(includeProperty);
                }
            }

            return querry.FirstOrDefault();
        }

        public void Remove(int Id)
        {
            T entityToRemove = Get(Id);
            Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            Remove(entity);
        }
    }
}
