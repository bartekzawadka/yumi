namespace Yumi.Infrastructure.Queries;

public record GetListQuery
{
    public string SearchPhrase { get; init; } = string.Empty;

    public int PageIndex { get; init; } = 0;

    public int PageSize { get; init; } = 10;

    public DateTime? From { get; init; }

    public DateTime? To { get; init; }
}