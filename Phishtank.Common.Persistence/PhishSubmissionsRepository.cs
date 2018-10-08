using Microsoft.Extensions.Configuration;
using Phishtank.Common.Entities;
using Phishtank.Common.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence
{
    public class PhishSubmissionsRepository : DocumentDbRepositoryBase, IPhishSubmissionsRepository
    {
        private const string _databaseName = "phishtank";
        private const string _collection = "submissions";

        public PhishSubmissionsRepository(IConfiguration configuration) : base(configuration)
        {
            DatabaseName = _databaseName;
            CollectionId = _collection;
            PartitionId = "/id";
        }

        public async Task<bool> IsPhishAsync(string host)
        {
            var query = GetQuery<Phish>(null).Where(p => p.Id == host);

            var results = await RunQueryAsync(query, null, null);

            if (results == null || results.Count == 0)
                return false;

            return true;
        }
    }
}
