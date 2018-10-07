using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Phish.Hosting.Functions.PhishDb
{
    public static class DatabaseRefreshFunction
    {
        [FunctionName("fetch_submissions")]
        public static void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
