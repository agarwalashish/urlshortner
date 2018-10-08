using Phishtank.Common.Entities.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence.Abstractions
{
    public interface IUrlRepository
    {
        Task AddUrlAsync(ShortUrl shortUrl);
        Task InitializeAsync();
    }
}
