using Learn.DurableFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Learn.DurableFunction.ActivityFunctions
{
    public static class ReportRepoViewCount
    {
        [FunctionName(nameof(ReportRepoViewCount))]
        public static async Task Run([ActivityTrigger] IDurableActivityContext context,
                                     ILogger log)
        {
            var repoViewCounts = context.GetInput<RepoViewCount[]>();
            foreach (var repoViewCount in repoViewCounts)
            {
                log.LogInformation($"Repository {repoViewCount.RepoName}'s view count is {repoViewCount.ViewCount}");
            }
        }
    }
}
