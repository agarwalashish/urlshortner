using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Persistence.Abstractions;
using System;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence
{
    public class UrlRepository : DocumentDbRepositoryBase, IUrlRepository
    {
        private const string _databaseName = "phishtank";
        private const string _collection = "shorturls";

        private readonly ILogger _logger;

        public UrlRepository(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
        {
            DatabaseName = _databaseName;
            CollectionId = _collection;
            PartitionId = "/id";

            _logger = loggerFactory != null ?
                      loggerFactory.CreateLogger(typeof(UrlRepository).GetType().Name) :
                      throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Adds a short url record to the persistent storage
        /// </summary>
        public Task AddUrlAsync(ShortUrl shortUrl)
        {
            return SaveDocumentAsync(shortUrl);
        }
    }
}
