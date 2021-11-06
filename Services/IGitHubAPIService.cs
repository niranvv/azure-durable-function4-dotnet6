using Learn.DurableFunction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learn.DurableFunction.Services
{
    public interface IGitHubAPIService
    {
        Task<List<string>> GetUserRepositoryList();
        Task<RepoViewCount> GetRepositoryViewCount(string repoName);
    }
}
