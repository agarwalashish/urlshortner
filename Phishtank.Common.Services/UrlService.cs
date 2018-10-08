using Phishtank.Common.Entities;
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

        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
        }

        public async Task<string> ShortenUrlAsync(string longUrl)
        {
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
