using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Learn.DurableFunction.TriggerFunctions
{
    public static class HttpTrigger
    {
        [FunctionName(nameof(HttpTrigger))]
        public static async Task<HttpResponseMessage> HttpStart([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
                                                                [DurableClient] IDurableOrchestrationClient starter,
                                                                ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DemoOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
