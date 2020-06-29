using Microsoft.EntityFrameworkCore;
using APP.Data;
using APP.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP.Repo
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public T Get(long id)
        {
            return entities.Find(id);
        }
        public DbSet<T> GetQueryable()
        {
            return entities;
        }
        public IQueryable<T> GetQueryable(long id)
        {
            return entities.Where(x=>x.Id==id).AsQueryable();
        }
        public long Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            SaveChange();
            return entity.Id;
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            SaveChange();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            SaveChange();
        }
        private void SaveChange()
        {
            context.SaveChanges();
        }
    }
}
