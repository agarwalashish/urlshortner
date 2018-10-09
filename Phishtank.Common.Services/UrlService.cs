using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Exceptions;
using Phishtank.Common.Persistence.Abstractions;
using Phishtank.Common.Services.Abstractions;
using Phishtank.Common.Utilities;
using System;
using System.Threading.Tasks;

namespace Phishtank.Common.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IPhishSubmissionsRepository _phishSubmissionsRepository;

        public UrlService(IUrlRepository urlRepository, IPhishSubmissionsRepository phishSubmissionsRepository)
        {
            _urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            _phishSubmissionsRepository = phishSubmissionsRepository ?? throw new ArgumentNullException(nameof(phishSubmissionsRepository));
        }

        public async Task<string> ShortenUrlAsync(ShortenUrlRequest request)
        {
            var uri = new Uri(request.LongUrl);

            // Check if the url is a known phishing site
            var isPhish = await _phishSubmissionsRepository.IsPhishAsync(uri.Host);

            if (isPhish)
            {
                //TODO: Flag ip address
                throw new InvalidShortenUrlRequestException($"The URL {request.LongUrl} is flagged for phishing");
            }

            var encodedUrl = request.LongUrl;

            var shortUrl = new ShortUrl
            {
                Id = Guid.NewGuid(),
                ShortenUrl = encodedUrl,
                LongUrl = request.LongUrl,
                CreatedOn = DateTimeOffset.UtcNow
            };

            await _urlRepository.AddUrlAsync(shortUrl);

            return encodedUrl;
        }
    }
}
