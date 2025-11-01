using System.Linq.Expressions;

namespace Salhia.KidsLibrary.Application.Common.Models;

public class QueryParameters<T>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Expression<Func<T, object>>? OrderBy { get; set; }
    public bool Descending { get; set; }
    public Expression<Func<T, bool>>? Filter { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; set; } = new();

}
