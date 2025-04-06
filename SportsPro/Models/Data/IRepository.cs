using System.Linq;

namespace SportsPro.Models.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> List(QueryOptions<T> options);
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
