using Learn.DurableFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learn.DurableFunction.ActivityFunctions
{
    public class GetUserRepositoryList
    {
        private readonly IGitHubAPIService _gitHubAPIService;

        public GetUserRepositoryList(IGitHubAPIService gitHubAPIService)
        {
            _gitHubAPIService = gitHubAPIService;
        }

        [FunctionName(nameof(GetUserRepositoryList))]
        public async Task<List<string>> Run(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log
            )
        {
            return await _gitHubAPIService.GetUserRepositoryList();
        }
    }
}
