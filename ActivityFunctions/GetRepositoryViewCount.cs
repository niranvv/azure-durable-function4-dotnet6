using Learn.DurableFunction.Models;
using Learn.DurableFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Learn.DurableFunction.ActivityFunctions
{
    public class GetRepositoryViewCount
    {
        private readonly IGitHubAPIService _gitHubAPIService;

        public GetRepositoryViewCount(IGitHubAPIService gitHubAPIService)
        {
            this._gitHubAPIService = gitHubAPIService;
        }

        [FunctionName(nameof(GetRepositoryViewCount))]
        public async Task<RepoViewCount> Run([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var repoName = context.GetInput<string>();
            return await _gitHubAPIService.GetRepositoryViewCount(repoName);
        }
    }
}
