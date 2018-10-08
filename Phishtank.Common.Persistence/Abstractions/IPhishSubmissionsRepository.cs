using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Common.Persistence.Abstractions
{
    public interface IPhishSubmissionsRepository
    {
        Task<bool> IsPhishAsync(string host);
        Task InitializeAsync();
    }
}
