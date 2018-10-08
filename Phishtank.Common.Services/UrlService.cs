using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Persistence.Abstractions;
using Phishtank.Common.Services.Abstractions;
using Phishtank.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<string> ShortenUrlAsync(string longUrl)
        {
            // Check if the url is a known phishing site
            var uri = new Uri(longUrl);
            var isPhish = await _phishSubmissionsRepository.IsPhishAsync(uri.Host);

            if (isPhish)
            {
                // deal with this here
            }

            var rand = new Random();
            var num = rand.Next();

            var encodedUrl = UrlUtils.Encode(num);

            var shortUrl = new ShortUrl
            {
                Id = Guid.NewGuid(),
                ShortenUrl = encodedUrl,
                LongUrl = longUrl,
                CreatedOn = DateTimeOffset.UtcNow
            };

            await _urlRepository.AddUrlAsync(shortUrl);

            return encodedUrl;
        }
    }
}
