using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Persistence.Abstractions;
using Phishtank.Common.Services;
using Phishtank.Common.Services.Abstractions;
using Phishtank.Tests.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Phishtank.Tests
{
    public class UrlServiceTests
    {
        private readonly IUrlRepository _mockUrlRepository;
        private readonly IPhishSubmissionsRepository _mockPhishSubmissionsRepository;
        private readonly IUrlService _urlService;

        public UrlServiceTests()
        {
            _mockUrlRepository = new MockUrlRepository();
            _mockPhishSubmissionsRepository = new MockPhishSubmissionsRepository();
            _urlService = new UrlService(_mockUrlRepository, _mockPhishSubmissionsRepository);
        }

        [Fact]
        public async Task ValidUrlTest()
        {
            var url = "https://www.google.com";

            await _urlService.ShortenUrlAsync(new ShortenUrlRequest { LongUrl = url });
        }
    }
}
