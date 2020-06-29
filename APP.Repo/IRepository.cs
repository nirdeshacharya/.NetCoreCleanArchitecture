using APP.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APP.Repo
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        T Get(long id);
        IQueryable<T> GetQueryable(long id);
        DbSet<T> GetQueryable();
        long Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
