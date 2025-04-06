using System;
using System.Linq.Expressions;

namespace SportsPro.Models.Data
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, bool>> Where { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }

        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}
