using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Exceptions;
using Phishtank.Common.Persistence.Abstractions;
using Phishtank.Common.Services;
using Phishtank.Common.Services.Abstractions;
using Phishtank.Tests.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Phishtank.Tests
{
    /// <summary>
    /// Test the service responsible for checking the url for phishing and shortening it
    /// </summary>
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

        /// <summary>
        /// Check for valid url
        /// </summary>
        [Fact]
        [Trait("Phistank.Common.Services", "UrlService")]
        public async Task ValidUrlTest()
        {
            var url = "https://www.google.com";
            await _urlService.ShortenUrlAsync(new ShortenUrlRequest { LongUrl = url });
        }

        /// <summary>
        /// Ensure that a known phishing url returns a 400 response
        /// </summary>
        [Fact]
        [Trait("Phistank.Common.Services", "UrlService")]
        public async Task PhishUrlTest()
        {
            var url = "https://secure.32fd20d33caaa96a435b8e7e74e243c2.cf/facebook.com/login-account.html";

            await Assert.ThrowsAsync<InvalidShortenUrlRequestException>(() => 
                _urlService.ShortenUrlAsync(new ShortenUrlRequest { LongUrl = url }));
        }

        /// <summary>
        /// Ensure that a invalid URI returns a 400 response
        /// </summary>
        [Fact]
        [Trait("Phistank.Common.Services", "UrlService")]
        public async Task InvalidUrlTest()
        {
            var url = "google.com";

            await Assert.ThrowsAsync<InvalidShortenUrlRequestException>(() =>
                _urlService.ShortenUrlAsync(new ShortenUrlRequest { LongUrl = url }));
        }

        /// <summary>
        /// Ensure that an empty url returns a 400 response
        /// </summary>
        [Fact]
        [Trait("Phistank.Common.Services", "UrlService")]
        public async Task EmptyUrlTest()
        {
            var url = string.Empty;

            await Assert.ThrowsAsync<InvalidShortenUrlRequestException>(() =>
                _urlService.ShortenUrlAsync(new ShortenUrlRequest { LongUrl = url }));
        }

        /// <summary>
        /// Ensure that a null request returns a 400 response
        /// </summary>
        [Fact]
        [Trait("Phistank.Common.Services", "UrlService")]
        public async Task EmptyRequestTest()
        {
            await Assert.ThrowsAsync<InvalidShortenUrlRequestException>(() =>
                _urlService.ShortenUrlAsync(null));
        }
    }
}
