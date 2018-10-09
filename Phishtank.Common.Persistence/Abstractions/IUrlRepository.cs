using Phishtank.Common.Entities.Internal;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence.Abstractions
{
    public interface IUrlRepository
    {
        /// <summary>
        /// Adds a short url record to the persistent storage
        /// </summary>
        /// <param name="shortUrl">ShortUrl document</param>
        Task AddUrlAsync(ShortUrl shortUrl);

        /// <summary>
        /// Performs the setup operations for the database if it does not already exist
        /// </summary>
        Task InitializeAsync();
    }
}
