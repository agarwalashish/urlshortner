using System;
using System.Collections.Generic;
using System.Text;

namespace Phishtank.Common.Entities.Internal
{
    public class ShortenUrlRequest
    {
        public string LongUrl { get; set; }

        public string IpAddress { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
