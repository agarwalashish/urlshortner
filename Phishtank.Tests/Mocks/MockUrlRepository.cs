using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Tests.Mocks
{
    public class MockUrlRepository : IUrlRepository
    {
        private readonly IList<ShortUrl> _shortUrls;

        public MockUrlRepository()
        {
            _shortUrls = new List<ShortUrl>();
        }

        public async Task AddUrlAsync(ShortUrl shortUrl)
        {
            _shortUrls.Add(shortUrl);
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
