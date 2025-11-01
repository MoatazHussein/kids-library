namespace Salhia.KidsLibrary.Application.Common.Models;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ItemsFrom = totalCount == 0 ? 0 : pageSize * (pageNumber - 1) + 1;
        ItemsTo = Math.Min(TotalItemsCount, pageNumber * pageSize);
    }

    public IEnumerable<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}