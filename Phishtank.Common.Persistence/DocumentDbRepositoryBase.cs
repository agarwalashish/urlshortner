using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence
{
    public class DocumentDbRepositoryBase : IDisposable
    {
        protected string DatabaseName;
        protected string CollectionId;
        protected string PartitionId;

        private readonly IConfiguration _configuration;
        private readonly string _authKey;
        private readonly string _dbUri;

        private DocumentClient _client;

        public DocumentDbRepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _authKey = _configuration["CosmosDb:AuthorizationKey"];
            _dbUri = _configuration["CosmosDb:Uri"];
        }

        public async Task<dynamic> GetDocumentAsyc(string objectId, string partitionKey)
        {
            var documentUri = UriFactory.CreateDocumentUri(DatabaseName, CollectionId, objectId);
            var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(partitionKey) };

            try
            {
                var response = await _client.ReadDocumentAsync(documentUri, requestOptions);
                return (dynamic)response.Resource;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                throw;
            }
        }

        public IQueryable<T> GetQuery<T>(string partitionKey) where T : class
        {
            var feedOptions = new FeedOptions { MaxItemCount = -1 };

            // If partitionKey has not been provided, then search across partitions
            if (!string.IsNullOrEmpty(partitionKey))
                feedOptions.PartitionKey = new PartitionKey(partitionKey);
            else
                feedOptions.EnableCrossPartitionQuery = true;

            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionId);
            var query = _client.CreateDocumentQuery<T>(collectionUri, feedOptions);

            return query;
        }

        public async Task<IList<T>> RunQueryAsync<T>(IQueryable<T> query, int? top, int? skip)
        {
            var exec = query.AsDocumentQuery();

            var documents = new List<T>();
            var skipped = 0;
            var take = (top != null && top > 0) ? top : 10;

            while (exec.HasMoreResults)
            {
                var result = await exec.ExecuteNextAsync<T>();

                if (result == null)
                    break;

                foreach (var document in result)
                {
                    if (skip != null && skip > 0)
                    {
                        if (skipped < skip)
                        {
                            skipped++;
                            continue;
                        }
                    }

                    if (documents.Count() < take)
                        documents.Add(document);
                    else
                        break;
                }
            }

            return documents;
        }

        public async Task SaveDocumentAsync(object document)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionId);
            await _client.UpsertDocumentAsync(collectionUri, document);
        }

        /// <summary>
        /// Performs the setup operations for the database if it does not already exist
        /// </summary>
        public async Task InitializeAsync()
        {
            _client = new DocumentClient(new Uri(_dbUri), _authKey);

            var databaseResponse = await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
            var database = databaseResponse.Resource;

            var indexingPolicy = new IndexingPolicy { IndexingMode = IndexingMode.Consistent };
            indexingPolicy.IncludedPaths.Add(new IncludedPath
            {
                Path = "/*",
                Indexes = new Collection<Index>()
                {
                    new RangeIndex(DataType.Number) {Precision = -1},
                    new RangeIndex(DataType.String) {Precision = -1},
                }
            });

            var collection = new DocumentCollection
            {
                Id = CollectionId,
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>()
                        {
                            PartitionId
                        }
                },
                IndexingPolicy = indexingPolicy
            };

            await _client.CreateDocumentCollectionIfNotExistsAsync(database.SelfLink, collection);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
