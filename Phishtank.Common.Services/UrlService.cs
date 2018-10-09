using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Exceptions;
using Phishtank.Common.Persistence.Abstractions;
using Phishtank.Common.Services.Abstractions;
using System;
using System.Threading.Tasks;

namespace Phishtank.Common.Services
{
    /// <summary>
    /// URL service to shorten urls. The service also implements checks to ensure that a given url is not a phishing url
    /// </summary>
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IPhishSubmissionsRepository _phishSubmissionsRepository;

        public UrlService(IUrlRepository urlRepository, IPhishSubmissionsRepository phishSubmissionsRepository)
        {
            _urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            _phishSubmissionsRepository = phishSubmissionsRepository ?? throw new ArgumentNullException(nameof(phishSubmissionsRepository));
        }

        /// <summary>
        /// Shortens a url
        /// </summary>
        public async Task<string> ShortenUrlAsync(ShortenUrlRequest request)
        {
            if (request == null)
                throw new InvalidShortenUrlRequestException("ShortenUrlRequest is null");
            
            if (string.IsNullOrEmpty(request.LongUrl) || !Uri.TryCreate(request.LongUrl, UriKind.Absolute, out Uri uri))
                throw new InvalidShortenUrlRequestException("The URL is either missing or not a valid URI");

            // Check if the url is a known phishing site
            var isPhish = await _phishSubmissionsRepository.IsPhishAsync(uri.Host);

            if (isPhish)
            {
                //TODO: Flag ip address
                throw new InvalidShortenUrlRequestException($"The URL {request.LongUrl} is flagged for phishing");
            }

            // TODO: add logic for shortening the submitted URL
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
