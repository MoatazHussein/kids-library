using System.Linq.Expressions;

namespace Salhia.KidsLibrary.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            Expression<Func<T, object>> orderBy,
            bool descending)
        {
            // Handle value type boxing
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = Expression.Convert(Expression.Invoke(orderBy, parameter), typeof(object));
            var lambda = Expression.Lambda<Func<T, object>>(body, parameter);

            return descending
                ? query.OrderByDescending(lambda)
                : query.OrderBy(lambda);
        }
    }

}
