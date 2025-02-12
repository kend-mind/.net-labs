using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using System.Text;
using System.Linq;
using Serilog;
using static VPBANK.RMD.Utils.Common.Enums;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Utils.Common.Helpers.Paging;
using VPBANK.RMD.Utils.Common.Shared;
using VPBANK.RMD.Utils.Common.Extensions;

namespace VPBANK.RMD.EFCore.Generics
{
    /// <summary>
    /// Generic Repository
    /// extending IRepository<TEntity> and/or ITrackableRepository<TEntity>, scope: application-wide
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TContext, TEntity, TKey> : IRepository<TContext, TEntity, TKey>, IDomainRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TEntity Find(object[] keyValues, CancellationToken cancellationToken = default);

        TEntity Find(TKey keyValues, CancellationToken cancellationToken = default);

        IEnumerable<TEntity> ListViewModels(string spName, object[] parameters);

        Task<IEnumerable<TEntity>> ListViewModelsAsync(string spName, object[] parameters);

        int CountDataPaginated(PaginatedInputModel paginatedInputModel);

        IEnumerable<TEntity> QueryDataPaginated(PaginatedInputModel paginatedInputModel);
    }

    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TContext, TEntity, TKey> : Repository<TContext, TEntity, TKey>, IGenericRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IDomainRepository<TContext, TEntity, TKey> _domainRepository;

        public GenericRepository(IDomainRepository<TContext, TEntity, TKey> domainRepository) : base(domainRepository)
        {
            this._domainRepository = domainRepository;
        }

        public TContext GetContext() => _domainRepository.GetContext();

        public TEntity Find(object[] keyValues, CancellationToken cancellationToken = default)
        {
            return this._domainRepository.Find(keyValues, cancellationToken);
        }

        public TEntity Find(TKey keyValues, CancellationToken cancellationToken = default)
        {
            return this._domainRepository.Find(keyValues, cancellationToken);
        }

        public void BulkInsert(IList<object> entities, CancellationToken cancellationToken = default)
        {
            this._domainRepository.BulkInsert(entities);
        }

        public async Task BulkInsertAsync(IList<object> entities, CancellationToken cancellationToken = default)
        {
            await this._domainRepository.BulkInsertAsync(entities);
        }

        public void BulkUpdate(IList<object> entities, CancellationToken cancellationToken = default)
        {
            this._domainRepository.BulkUpdate(entities);
        }

        public async Task BulkUpdateAsync(IList<object> entities, CancellationToken cancellationToken = default)
        {
            await this._domainRepository.BulkUpdateAsync(entities);
        }

        public void BulkDelete(IList<object> entities, CancellationToken cancellationToken = default)
        {
            this._domainRepository.BulkDelete(entities);
        }

        public async Task BulkDeleteAsync(IList<object> entities, CancellationToken cancellationToken = default)
        {
            await this._domainRepository.BulkDeleteAsync(entities);
        }

        // implement get list by store procedure
        public IEnumerable<TEntity> ListViewModels(string spName, object[] parameters)
        {
            try
            {
                var result = _domainRepository.QueryableSql(spName, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // implement async get list by store procedure
        public async Task<IEnumerable<TEntity>> ListViewModelsAsync(string spName, object[] parameters)
        {
            try
            {
                var results = await this.TrackableRepository.Query().SelectSqlAsync(spName, parameters);
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CountDataPaginated(PaginatedInputModel paginatedInputModel)
        {
            var sqlQuery = BuildSqlQuery(false, paginatedInputModel);
            var total = TrackableRepository.QueryableSql(sqlQuery).Count();
            return total;
        }

        public IEnumerable<TEntity> QueryDataPaginated(PaginatedInputModel paginatedInputModel)
        {
            try
            {
                var sqlQuery = BuildSqlQuery(true, paginatedInputModel);
                var results = TrackableRepository.QueryableSql(sqlQuery).AsEnumerable();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string BuildSqlQuery(bool isQuery, PaginatedInputModel paginatedInputModel)
        {
            var context = _domainRepository.GetContext();
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var tableName = entityType.GetTableName();
            var tableSchema = entityType.GetSchema();

            var query = new StringBuilder();

            // body query
            query.Append($"SELECT {SpecificSystems.BULLET}");
            query.Append($" FROM [{tableSchema}].[{tableName}]");

            // filters
            var filters = paginatedInputModel.FilterExpression;
            if (!string.IsNullOrEmpty(filters))
                query.Append($" WHERE {filters}");

            if (isQuery)
            {
                // sorting
                var sorts = new List<string>();
                if (paginatedInputModel.SortingParams != null && paginatedInputModel.SortingParams.Any())
                    foreach (var item in paginatedInputModel.SortingParams)
                        sorts.Add($"[{item.ColumnName}] {item.SortOrder.GetDescription()}");
                else
                    sorts.Add($"{EfCoreConstants.Pk_Id} {SortOrders.Asc.GetDescription()}");

                if (sorts != null && sorts.Any())
                    query.Append($" ORDER BY {string.Join(SpecificSystems.COL_JOIN_COMMA, sorts)}");

                // paging
                var offSet = paginatedInputModel.PageIndex * paginatedInputModel.PageSize;
                var fetchNext = paginatedInputModel.PageSize;
                query.Append($" OFFSET {offSet} ROWS FETCH NEXT {fetchNext} ROWS ONLY");
            }

            Log.Information($"EXECUTE QUERY: {query}");
            return query.ToString();
        }
    }
}
