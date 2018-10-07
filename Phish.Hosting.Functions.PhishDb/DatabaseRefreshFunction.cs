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
        /// <summary>
        /// The az function will run once at the top of every hour to fetch the latest phish urls
        /// </summary>
        [FunctionName("fetch_submissions")]
        public static async Task Run(
                               [TimerTrigger("0 0 * * * *", RunOnStartup = true)]TimerInfo myTimer, 
                               ILogger log)
        {
            var phishTankDataUrl = Environment.GetEnvironmentVariable("PhishTankDataUrl");
            var phishTankApiKey = Environment.GetEnvironmentVariable("PhishTankApiKey");

            phishTankDataUrl = string.Format(phishTankDataUrl, phishTankApiKey);

            var docDbUrl = new Uri("https://localhost:8081");
            var docDbAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";


            var authKey = new SecureString
            {

            };

            using (var cosmosClient = new DocumentClient(docDbUrl, docDbAuthKey))
            {
                using (var httpClient = new HttpClient())
                {
                    var fetchResponse = await httpClient.GetAsync(phishTankDataUrl);
                    var fetchStr = await fetchResponse.Content.ReadAsStringAsync();

                    if (fetchResponse.IsSuccessStatusCode)
                    {
                        var phishes = JsonConvert.DeserializeObject<List<Phishtank.Common.Entities.Phish>>(fetchStr);

                        foreach (var phish in phishes)
                        {
                            var collectionUri = UriFactory.CreateDocumentCollectionUri("phishtank", "submissions");
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
//[CosmosDB("phishtank", "submissions", ConnectionStringSetting = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")] DocumentClient client, 