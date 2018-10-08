using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Phish.Hosting.Functions.PhishDb
{
    public static class DatabaseRefreshFunction
    {
        private const string DatabaseName = "phishtank";
        private const string CollectionName = "submissions";

        /// <summary>
        /// The az function will run once at the top of every hour to fetch the latest phish urls
        /// </summary>
        [FunctionName("fetch_submissions")]
        public static async Task Run([TimerTrigger("0 0 * * * *", RunOnStartup = true)]TimerInfo myTimer, 
                                      ILogger log)
        {
            var phishTankDataUrl = Environment.GetEnvironmentVariable("PhishTankDataUrl");
            var phishTankApiKey = Environment.GetEnvironmentVariable("PhishTankApiKey");
            var docDbUrl = new Uri(Environment.GetEnvironmentVariable("CosmosDbUrl"));
            var docDbAuthKey = Environment.GetEnvironmentVariable("CosmosDbAuthKey");

            phishTankDataUrl = string.Format(phishTankDataUrl, phishTankApiKey);

            using (var cosmosClient = new DocumentClient(docDbUrl, docDbAuthKey))
            {
                using (var httpClient = new HttpClient())
                {
                    var fetchResponse = await httpClient.GetAsync(phishTankDataUrl);
                    var fetchStr = await fetchResponse.Content.ReadAsStringAsync();

                    if (fetchResponse.IsSuccessStatusCode)
                    {
                        var phishes = JsonConvert.DeserializeObject<List<Phishtank.Common.Entities.Internal.Phish>>(fetchStr);

                        foreach (var phish in phishes)
                        {
                            if (string.IsNullOrWhiteSpace(phish.Url))
                                continue;

                            var uri = new Uri(phish.Url);
                            phish.Id = uri.Host;

                            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
                            await cosmosClient.UpsertDocumentAsync(collectionUri, phish);
                        }
                    }
                    else
                    {
                        log.LogError(fetchStr);
                    }
                }
            }
        }
    }
}