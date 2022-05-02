using Raven.Client.Documents;
using Yumi.Infrastructure.Sys;

namespace Yumi.Infrastructure.Extensions;

public static class CollectionExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> queryable,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var count = await queryable.CountAsync(cancellationToken);
        var pIndex = pageIndex < 0 ? 0 : pageIndex;
        var pSize = pageSize <= 0 ? 10 : pageSize;

        queryable = queryable.Skip(pIndex * pSize).Take(pSize);
        var data = await queryable.ToListAsync(cancellationToken);
        return new PagedList<T>(data, count, pIndex, pSize);
    }
}