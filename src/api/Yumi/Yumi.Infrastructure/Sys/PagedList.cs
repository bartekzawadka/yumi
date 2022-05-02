namespace Yumi.Infrastructure.Sys;

public class PagedList<T>
{
    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalPages { get; }

    public long TotalCount { get; }

    public IEnumerable<T> Data { get; }

    public PagedList(IEnumerable<T> items, long totalCount, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalCount = totalCount;

        Data = items;
    }
}