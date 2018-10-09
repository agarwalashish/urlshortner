using Phishtank.Common.Entities.Internal;
using Phishtank.Common.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phishtank.Tests.Mocks
{
    internal class MockPhishSubmissionsRepository : IPhishSubmissionsRepository
    {
        private readonly IList<Phish> _phishSubmissions;

        public MockPhishSubmissionsRepository()
        {
            _phishSubmissions = new List<Phish>
            {
                new Phish
                {
                    Id = "secure.32fd20d33caaa96a435b8e7e74e243c2.cf",
                    Url = "https://secure.32fd20d33caaa96a435b8e7e74e243c2.cf/facebook.com/login-account.html",
                    PhishId = "5807814"
                }
            };
        }

        public async Task<bool> IsPhishAsync(string host)
        {
            return _phishSubmissions.Any(ph => ph.Id == host);
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
