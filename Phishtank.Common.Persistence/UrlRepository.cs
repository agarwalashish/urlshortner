using Microsoft.Extensions.Configuration;
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

        public UrlRepository(IConfiguration configuration) : base(configuration)
        {
            DatabaseName = _databaseName;
            CollectionId = _collection;
            PartitionId = "/id";
        }

        public Task AddUrlAsync(ShortUrl shortUrl)
        {
            return SaveDocumentAsync(shortUrl);
        }
    }
}
