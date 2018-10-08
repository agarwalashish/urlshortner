using System;
using System.Collections.Generic;
using System.Text;

namespace Phishtank.Common.Entities
{
    public class ShortUrl
    {
        public Guid Id { get; set; }

        public int ShortId { get; set; }

        public string LongUrl { get; set; }

        public string ShortenUrl { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
