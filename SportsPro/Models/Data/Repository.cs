using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsPro.Models.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private SportsProContext context;
        private DbSet<T> dbSet;

        public Repository(SportsProContext ctx)
        {
            context = ctx;
            dbSet = context.Set<T>();
        }

        public IQueryable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet;

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
                query = query.OrderBy(options.OrderBy);

            return query;
        }

        public T Get(int id) => dbSet.Find(id);
        public void Insert(T entity) => dbSet.Add(entity);
        public void Update(T entity) => dbSet.Update(entity);
        public void Delete(T entity) => dbSet.Remove(entity);
        public void Save() => context.SaveChanges();
    }
}
