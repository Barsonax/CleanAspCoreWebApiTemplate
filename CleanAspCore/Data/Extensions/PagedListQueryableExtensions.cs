using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Data.Extensions;

public static class PagedListQueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var count = await source.CountAsync(token);
        if (count > 0)
        {
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);
            return new PagedList<T>(items, count, page, pageSize);
        }

        return new(Enumerable.Empty<T>(), 0, 0, pageSize);
    }
}

public class PagedList<T>
{
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalRecords = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Data = items as IList<T> ?? new List<T>(items);
    }

    public IList<T> Data { get; init; }
    public int PageNumber { get; init; }
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
}
