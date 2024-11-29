namespace CleanAspCore.Core.Common.Paging;

public sealed class PagedList<T>
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
