using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phishtank.Common.Entities
{
    public class Phish
    {
        [JsonProperty("phish_id")]
        public string id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("phish_detail_url")]
        public string DetailUrl { get; set; }

        [JsonProperty("submission_time")]
        public DateTimeOffset? SubmissionTime { get; set; }

        [JsonProperty("verified")]
        public string Verified { get; set; }

        [JsonProperty("online")]
        public string Online { get; set; }

        [JsonProperty("verification_time")]
        public DateTimeOffset? VerificationTime { get; set; }

        [JsonProperty("details")]
        public IList<PhishDetails> Details { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }
}
