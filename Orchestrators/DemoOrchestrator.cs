using Learn.DurableFunction.ActivityFunctions;
using Learn.DurableFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learn.DurableFunction.Orchestrators
{
    public static class DemoOrchestrator
    {
        [FunctionName(nameof(DemoOrchestrator))]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var outputs = new List<string>();
            var repos = await context.CallActivityAsync<List<string>>(nameof(GetUserRepositoryList), null);

            var list = string.Join(',', repos);
            log.LogInformation($"Repository list: {list}");

            var tasks = new List<Task<RepoViewCount>>();
            //fan-out
            foreach (var repo in repos)
            {
                log.LogInformation($"Adding Task for Repository {repo}");
                var task = context.CallActivityAsync<RepoViewCount>(nameof(GetRepositoryViewCount), repo);
                tasks.Add(task);
            }

            //fan-in
            var repoViewCounts = await Task.WhenAll(tasks);
            await context.CallActivityAsync(nameof(ReportRepoViewCount), repoViewCounts);
        }
    }
}