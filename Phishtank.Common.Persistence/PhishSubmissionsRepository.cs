using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Persistence.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence
{
    public class PhishSubmissionsRepository : DocumentDbRepositoryBase, IPhishSubmissionsRepository
    {
        private const string _databaseName = "phishtank";
        private const string _collection = "submissions";

        private readonly ILogger _logger;

        public PhishSubmissionsRepository(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
        {
            DatabaseName = _databaseName;
            CollectionId = _collection;
            PartitionId = "/id";

            _logger = loggerFactory != null ?
                      loggerFactory.CreateLogger(typeof(PhishSubmissionsRepository).GetType().Name) :
                      throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Checks if the given URL has been flagged as a phish in our database
        /// </summary>
        /// <param name="host">Host of the url</param>
        public async Task<bool> IsPhishAsync(string host)
        {
            var query = GetQuery<Phish>(null).Where(p => p.Id == host);

            var results = await RunQueryAsync(query, null, null);

            // If there were no results found in the database, it means that the URL is not a known phish
            if (results == null || results.Count == 0)
                return false;

            return true;
        }
    }
}
