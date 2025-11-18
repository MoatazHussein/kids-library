using System.Linq.Expressions;
using Salhia.KidsLibrary.Application.Common.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Salhia.KidsLibrary.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default,  params Expression<Func<T, object>>[] includes);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate,CancellationToken cancellationToken = default,params Expression<Func<T, object>>[] includes);
    Task<(IEnumerable<T>, int)> GetAllMatchingAsync(QueryParameters<T> parameters, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity);
    public Task<int> BulkUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
