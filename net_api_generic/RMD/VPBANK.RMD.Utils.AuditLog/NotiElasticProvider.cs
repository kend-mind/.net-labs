using Elasticsearch.Net;
using VPBANK.RMD.Utils.AuditLog.Models;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Shared;
using Serilog;

namespace VPBANK.RMD.Utils.AuditLog
{
    public interface INotiElasticProvider<T>
    {
        long Count(ElasticSearchPaging auditTrailParam = null);
        Task<long> CountAsync(ElasticSearchPaging auditTrailParam = null);
        void AddLog(T elasticLog);
        Task AddLogAsync(T elasticLog);
        void AddManyLog(List<T> elasticLogs);
        Task AddManyLogAsync(List<T> elasticLogs);
        IEnumerable<T> QueryAuditLogs(ElasticSearchPaging auditTrailParam = null);
        Task<IEnumerable<T>> QueryAuditLogsAsync(ElasticSearchPaging auditTrailParam = null);
    }

    public class NotiElasticProvider<T> : INotiElasticProvider<T> where T : class
    {
        private string _alias = string.Empty;
        private string _indexName = string.Empty;
        private readonly IOptions<ElasticSearchOptions> _options;

        private ElasticClient _elasticClient { get; }

        public NotiElasticProvider(IOptions<ElasticSearchOptions> elasticOptions)
        {
            _options = elasticOptions ?? throw new ArgumentNullException(nameof(elasticOptions));

            _alias = _options.Value.NotiAlias;
            if (_options.Value.FixAlias)
                _indexName = _alias;
            else
                _indexName = _options.Value.IndexPerYear
                        ? $"{_alias}_{DateTime.Now.ToString(DefFormats.DATE_YYYY)}"
                        : $"{_alias}_{DateTime.Now.ToString(DefFormats.DATE_YYYYMM)}";

            var pool = new StaticConnectionPool(new List<Uri> { new Uri(_options.Value.Uri) });
            var connectionSettings = new ConnectionSettings(pool).DefaultMappingFor<T>(m => m.IndexName(_indexName));

            //new HttpConnection(),
            //new SerializerFactory((jsonSettings, nestSettings) => jsonSettings.Converters.Add(new StringEnumConverter()))).DisableDirectStreaming();

            _elasticClient = new ElasticClient(connectionSettings);
        }

        public long Count(ElasticSearchPaging elasticParam = null)
        {
            var searchRequest = BuildSearchRequest(elasticParam);
            return _elasticClient.Search<T>(searchRequest).Total;
        }

        public async Task<long> CountAsync(ElasticSearchPaging elasticParam = null)
        {
            var searchRequest = BuildSearchRequest(elasticParam);
            var searchResponse = await _elasticClient.SearchAsync<T>(searchRequest);
            return searchResponse.Total;
        }

        public void AddLog(T elasticLog)
        {
            var indexRequest = new IndexRequest<T>(elasticLog);
            var response = _elasticClient.Index(indexRequest);
            Log.Information(response.DebugInformation);
            if (!response.IsValid)
                throw new ElasticsearchClientException("Add vpb_rmd_auditlog disaster!");
        }

        public async Task AddLogAsync(T elasticLog)
        {
            var indexRequest = new IndexRequest<T>(elasticLog);
            var response = await _elasticClient.IndexAsync(indexRequest);
            Log.Information(response.DebugInformation);
            if (!response.IsValid)
                throw new ElasticsearchClientException("Add vpb_rmd_auditlog disaster!");
        }

        public void AddManyLog(List<T> elasticLogs)
        {
            var response = _elasticClient.IndexMany(elasticLogs);
            Log.Information(response.DebugInformation);
            if (!response.IsValid)
                throw new ElasticsearchClientException("Add vpb_rmd_auditlog disaster!");
        }

        public async Task AddManyLogAsync(List<T> elasticLogs)
        {
            var response = await _elasticClient.IndexManyAsync(elasticLogs);
            Log.Information(response.DebugInformation);
            if (!response.IsValid)
                throw new ElasticsearchClientException("Add vpb_rmd_auditlog disaster!");
        }

        public IEnumerable<T> QueryAuditLogs(ElasticSearchPaging elasticParam = null)
        {
            var searchRequest = BuildSearchRequest(elasticParam);
            var searchResponse = _elasticClient.Search<T>(searchRequest);
            return searchResponse.Documents;
        }

        public async Task<IEnumerable<T>> QueryAuditLogsAsync(ElasticSearchPaging elasticParam = null)
        {
            var searchRequest = BuildSearchRequest(elasticParam);
            var searchResponse = await _elasticClient.SearchAsync<T>(searchRequest);
            return searchResponse.Documents;
        }

        private SearchRequest<T> BuildSearchRequest(ElasticSearchPaging elasticParam = null)
        {
            EnsureAlias();

            var query = new QueryContainer(new SimpleQueryStringQuery
            {
                Query = string.IsNullOrEmpty(elasticParam.Filters) ? SpecificSystems.BULLET : elasticParam.Filters,
                Fields = new Field(AuditElasticSearchs.FIELD_RECEIVER),
                Name = AuditElasticSearchs.FIELD_RECEIVER,
                DefaultOperator = Operator.And
            });

            var sort = new List<ISort> { new FieldSort { Field = AuditElasticSearchs.FIELD_RECEIVER_TIME, Order = SortOrder.Descending } };

            var searchRequest = new SearchRequest<T>(Indices.Parse(_alias))
            {
                From = (elasticParam == null || elasticParam.From <= 0) ? 0 : elasticParam.From * elasticParam.Size,
                Size = (elasticParam == null || elasticParam.Size <= 0) ? 25 : elasticParam.Size,
                Query = query,
                Sort = sort
            };

            return searchRequest;
        }

        private void CreateAlias()
        {
            if (_options.Value.AmountOfPreviousIndicesUsedInAlias > 0)
                CreateAliasForLastIndices(_options.Value.AmountOfPreviousIndicesUsedInAlias);
            else
                CreateAliasForAllIndices();
        }

        private void CreateAliasForAllIndices()
        {
            var response = _elasticClient.Indices.AliasExists(new AliasExistsRequest(new Names(new List<string> { _alias })));
            //if (!response.IsValid)
            //    throw response.OriginalException;

            if (response.Exists)
                _elasticClient.Indices.DeleteAlias(new DeleteAliasRequest(Indices.Parse($"{_alias}{SpecificSystems.BULLET}"), _alias));

            var responseCreateIndex = _elasticClient.Indices.PutAlias(new PutAliasRequest(Indices.Parse($"{_alias}{SpecificSystems.BULLET}"), _alias));
            if (!responseCreateIndex.IsValid)
                throw response.OriginalException;
        }

        private void CreateAliasForLastIndices(int amount)
        {
            var responseCatIndices = _elasticClient.Cat.Indices(new CatIndicesRequest(Indices.Parse($"{_alias}{SpecificSystems.BULLET}")));
            var records = responseCatIndices.Records.ToList();
            var indicesToAddToAlias = new List<string>();
            for (int i = amount; i > 0; i--)
            {
                if (_options.Value.FixAlias)
                {
                    var indexName = _alias;
                    if (records.Exists(t => t.Index == indexName))
                        indicesToAddToAlias.Add(indexName);
                }
                else
                {
                    if (_options.Value.IndexPerYear)
                    {
                        var indexName = $"{_alias}_{DateTime.Now.AddMonths(-i + 1).ToString(DefFormats.DATE_YYYY)}";
                        if (records.Exists(t => t.Index == indexName))
                            indicesToAddToAlias.Add(indexName);
                    }
                    else
                    {
                        var indexName = $"{_alias}_{DateTime.Now.AddDays(-i + 1).ToString(DefFormats.DATE_YYYYMM)}";
                        if (records.Exists(t => t.Index == indexName))
                            indicesToAddToAlias.Add(indexName);
                    }
                }
            }

            var response = _elasticClient.Indices.AliasExists(new AliasExistsRequest(new Names(new List<string> { _alias })));
            //if (!response.IsValid)
            //    throw response.OriginalException;

            if (response.Exists)
                _elasticClient.Indices.DeleteAlias(new DeleteAliasRequest(Indices.Parse($"{_alias}{SpecificSystems.BULLET}"), _alias));

            Indices multipleIndicesFromStringArray = indicesToAddToAlias.Distinct().ToArray();
            var responseCreateIndex = _elasticClient.Indices.PutAlias(new PutAliasRequest(multipleIndicesFromStringArray, _alias));
            if (!responseCreateIndex.IsValid)
                throw responseCreateIndex.OriginalException;
        }

        private static DateTime aliasUpdated = DateTime.Now.AddYears(-50);

        // TODO: fix environment
        private void EnsureAlias()
        {
            aliasUpdated = DateTime.Now.AddYears(-50);
            if (_options.Value.IndexPerYear)
            {
                if (aliasUpdated.Date < DateTime.UtcNow.AddMonths(-1).Date)
                {
                    aliasUpdated = DateTime.UtcNow;
                    CreateAlias();
                }
            }
            else
            {
                if (aliasUpdated.Date < DateTime.UtcNow.AddDays(-1).Date)
                {
                    aliasUpdated = DateTime.UtcNow;
                    CreateAlias();
                }
            }
        }
    }
}
