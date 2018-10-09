using Phishtank.Common.Entities.Internal;
using System.Threading.Tasks;

namespace Phishtank.Common.Services.Abstractions
{
    /// <summary>
    /// Interface for shortening urls. This interface also implements some security protocols to ensure that the url is not a known phishing url
    /// </summary>
    public interface IUrlService
    {
        /// <summary>
        /// Shortens a url
        /// </summary>
        /// <param name="request">Shorten URL request</param>
        Task<string> ShortenUrlAsync(ShortenUrlRequest request);
    }
}
