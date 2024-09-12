using System.Data;
using System.Linq.Expressions;

namespace Mypethere.User.Domain.Repositories;

public interface IRepository<T, REntity, TId> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction);
    Task<T> GetByIdAsync(TId id);
    Task CreateAsync(T entity);
    Task CreateAsync(T entity, ITransaction transaction);
    Task UpdateAsync(TId id, T entity, ITransaction transaction);
    Task DeleteAsync(TId id);
    Task<IEnumerable<T>> GetPaginated(int pageNumber, int pageSize, Expression<Func<REntity, bool>> filter);
    Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<REntity, bool>> filter);
    Task<bool> ExistsAsync(Expression<Func<REntity, bool>> filter);
    Task<T> GetSingleByConditionAsync(Expression<Func<REntity, bool>> filter);
}
