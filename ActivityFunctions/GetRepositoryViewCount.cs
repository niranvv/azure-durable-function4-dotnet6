using Learn.DurableFunction.Models;
using Learn.DurableFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
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
            ThrowRandomException();
            var repoName = context.GetInput<string>();
            return await _gitHubAPIService.GetRepositoryViewCount(repoName);
        }

        private static void ThrowRandomException()
        {
            var random = new Random();
            var next = random.Next(11);
            if (next >= 7)
            {
                throw new TooManyRequestsException("Random Excption thrown to test retry");
            }
        }
    }
}
