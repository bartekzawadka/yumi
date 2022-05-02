using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Yumi.Infrastructure.Extensions;
using Yumi.Infrastructure.Models;
using Yumi.Infrastructure.Queries;
using Yumi.Infrastructure.Sys;

namespace Yumi.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : DbDocument
{
    private readonly IAsyncDocumentSession _dbSession;

    public GenericRepository(IAsyncDocumentSession dbSession)
    {
        _dbSession = dbSession;
    }

    public Task InsertOrUpdateAsync(T document, CancellationToken cancellationToken) =>
        _dbSession.StoreAsync(document, document.Id, cancellationToken);

    public async Task InsertOrUpdateManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken)
    {
        foreach (var dbDocument in documents)
        {
            await _dbSession.StoreAsync(dbDocument, cancellationToken);
        }
    }

    public Task<PagedList<T>> GetPagedListAsync(int pageIndex, int pageSize, CancellationToken cancellationToken) =>
        _dbSession.Query<T>().ToPagedListAsync(pageIndex, pageSize, cancellationToken);

    public Task<List<TOut>> GetListAsAsync<TOut>(
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null) =>
        GetQueryable(selectExpression, filterExpression).ToListAsync(cancellationToken);

    public Task<PagedList<TOut>> GetListAsAsync<TQuery, TOut>(
        TQuery query,
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] searchFields) where TQuery : GetListQuery
    {
        var from = query.From?.ToUniversalTime();
        var to = query.To?.ToUniversalTime();

        var ravenQueryable = _dbSession.Query<T>();

        if (from != null)
        {
            ravenQueryable = ravenQueryable.Where(document => document.TimeStamp >= from);
        }

        if (to != null)
        {
            ravenQueryable = ravenQueryable.Where(document => document.TimeStamp < to);
        }

        if (!string.IsNullOrWhiteSpace(query.SearchPhrase))
        {
            ravenQueryable = searchFields.Aggregate(
                ravenQueryable,
                (current, searchField) => current.Search(searchField, query.SearchPhrase));
        }

        return ravenQueryable.Select(selectExpression).ToPagedListAsync(query.PageIndex, query.PageSize, cancellationToken);
    }

    public Task<List<T>> GetListAsync(
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null) =>
        GetQueryable(document => document, filterExpression).ToListAsync(cancellationToken);

    public Task<T> GetOneAsync(
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes) =>
        GetQueryable(document => document, filterExpression, orderBy, includes)
            .SingleOrDefaultAsync(cancellationToken);

    public Task<TOut> GetOneAsAsync<TOut>(
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes) =>
        GetQueryable(selectExpression, filterExpression, orderBy, includes).SingleOrDefaultAsync(cancellationToken);

    public Task<List<T>> GetSearchResultsAsync(
        string searchPhrase,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] searchFields)
    {
        var queryable = _dbSession.Query<T>();
        queryable = searchFields.Aggregate(
            queryable,
            (current, searchField) => current.Search(searchField, searchPhrase));

        return queryable.ToListAsync(cancellationToken);
    }

    public Task<PagedList<TOut>> GetPagedListAsAsync<TOut>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TOut>> selectExpression,
        CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes) =>
        GetQueryable(selectExpression, filterExpression, orderBy, includes)
            .ToPagedListAsync(pageIndex, pageSize, cancellationToken);

    public Task<T> GetByIdAsync(string? id, CancellationToken cancellationToken, params string[]? includedProperties)
    {
        if (includedProperties != null && includedProperties.Any())
        {
            foreach (var includedProperty in includedProperties)
            {
                _dbSession.Include(includedProperty);
            }
        }

        return _dbSession.LoadAsync<T>(id, cancellationToken);
    }

    public Task<bool> ExistsAsync(
        Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken) =>
        _dbSession.Query<T>().AnyAsync(filterExpression, cancellationToken);

    public void Delete(string? id) => _dbSession.Delete(id);

    public void Delete(T document) => _dbSession.Delete(document);

    public void Delete(IEnumerable<T> documents)
    {
        foreach (var dbDocument in documents)
        {
            Delete(dbDocument);
        }
    }

    public void Delete(IEnumerable<string> documentIds)
    {
        foreach (var documentId in documentIds)
        {
            Delete(documentId);
        }
    }

    public async Task DeleteByAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
    {
        var ids = await _dbSession
            .Query<T>()
            .Where(filter)
            .Select(document => document.Id)
            .ToListAsync(cancellationToken);

        foreach (var id in ids)
        {
            Delete(id);
        }
    }

    private IQueryable<TOut> GetQueryable<TOut>(
        Expression<Func<T, TOut>> selectExpression,
        Expression<Func<T, bool>>? filterExpression = null,
        Func<IRavenQueryable<T>, IRavenQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[]? includes)
    {
        var query = _dbSession.Query<T>();
        if (filterExpression != null)
        {
            query = query.Where(filterExpression);
        }

        if (includes != null && includes.Any())
        {
            query = includes.Aggregate(query, (documents, expression) => documents.Include(expression));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query.Select(selectExpression);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => _dbSession.SaveChangesAsync(cancellationToken);
}