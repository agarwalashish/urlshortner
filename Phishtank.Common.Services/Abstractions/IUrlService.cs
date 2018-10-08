using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Common.Services.Abstractions
{
    public interface IUrlService
    {
        Task<string> ShortenUrlAsync(string longUrl);
    }
}
