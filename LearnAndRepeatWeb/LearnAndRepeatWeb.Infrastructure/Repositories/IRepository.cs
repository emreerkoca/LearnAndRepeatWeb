using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> GetById<TId>(TId id) where TId : notnull;
        Task<List<T>> GetItems(Expression<Func<T, bool>> expression);
        Task<T> GetItem(Expression<Func<T, bool>> expression);
        Task Update(T entity);
        Task<bool> Any(Expression<Func<T, bool>> expression);
    }
}
