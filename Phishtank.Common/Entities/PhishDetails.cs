using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phishtank.Common.Entities
{
    public class PhishDetails
    {
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty("cidr_block")]
        public string CidrBlock { get; set; }

        [JsonProperty("announcing_network")]
        public string AnnouncingNetwork { get; set; }

        [JsonProperty("rir")]
        public string Rir { get; set; }

        [JsonProperty("country")]
        public string CountryCode { get; set; }

        [JsonProperty("detail_time")]
        public DateTimeOffset? DetailTime { get; set; }
    }
}
