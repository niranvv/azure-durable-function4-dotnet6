using Learn.DurableFunction.ActivityFunctions;
using Learn.DurableFunction.Exceptions;
using Learn.DurableFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Learn.DurableFunction.Orchestrators
{
    public static class GetViewCountOrchestrator
    {
        [FunctionName(nameof(GetViewCountOrchestrator))]
        public static async Task<RepoViewCount> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context,
                                                                ILogger log)
        {
            var repoName = context.GetInput<string>();
            try
            {
                return await context.CallActivityAsync<RepoViewCount>(nameof(GetRepositoryViewCount),repoName);
            }
            catch (Exception ex)
            {
                if (ex.InnerException!=null && ex.InnerException is TooManyRequestsException)
                {
                    Random random = new Random();
                    int delay = random.Next(10, 31);
                    log.LogInformation($"Retry after {delay} seconds.");
                    DateTime backoff = context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(delay));
                    await context.CreateTimer(backoff, System.Threading.CancellationToken.None);
                    var updatedRepoName = UpdateRepoName(repoName);
                    context.ContinueAsNew(updatedRepoName);
                }
                else
                {
                    throw;
                }
                return null;
            }
        }

        private static object UpdateRepoName(string input)
        {
            // change the input if required
            return input;
        }
    }
}