using System.Linq.Expressions;
using Raven.Client.Documents.Linq;
using Yumi.Infrastructure.Models;
using Yumi.Infrastructure.Queries;
using Yumi.Infrastructure.Sys;

namespace Yumi.Infrastructure.Repositories;

public interface IGenericRepository<T> where T : DbDocument
{
    Task InsertOrUpdateAsync(T document, CancellationToken cancellationToken);

    Task InsertOrUpdateManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken);

    Task<PagedList<T>> GetPagedListAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);

    Task<List<TOut>> GetListAsAsync<TOut>(
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null);

    Task<PagedList<TOut>> GetListAsAsync<TQuery, TOut>(
        TQuery query,
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] searchFields) where TQuery : GetListQuery;

    Task<List<T>> GetListAsync(
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null);

    Task<T> GetOneAsync(
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes);

    Task<TOut> GetOneAsAsync<TOut>(
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes);

    Task<List<T>> GetSearchResultsAsync(
        string searchPhrase,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] searchFields);

    Task<PagedList<TOut>> GetPagedListAsAsync<TOut>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes);

    Task<T> GetByIdAsync(string? id, CancellationToken cancellationToken, params string[]? includedProperties);

    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken);

    void Delete(string? id);

    void Delete(T document);

    void Delete(IEnumerable<T> documents);

    void Delete(IEnumerable<string> documentIds);

    Task DeleteByAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}