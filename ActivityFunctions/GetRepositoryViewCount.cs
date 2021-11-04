using Learn.DurableFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Learn.DurableFunction.ActivityFunctions
{
    public static class GetRepositoryViewCount
    {
        [FunctionName(nameof(GetRepositoryViewCount))]
        public static async Task<RepoViewCount> Run([ActivityTrigger] IDurableActivityContext context,
                                                    ILogger log)
        {
            var repoName = context.GetInput<string>();
            var result = new RepoViewCount { RepoName = repoName };
            var username = Environment.GetEnvironmentVariable("GitHubUsername");
            var password = Environment.GetEnvironmentVariable("GitHubPassword");

            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("C# agent");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            
            log.LogInformation($"Fetch View Count for {repoName}. Starting...");
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{username}/{repoName}/traffic/views");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(body);
            result.ViewCount = data.count;
            log.LogInformation($"GOT View Count for {repoName} as [{result.ViewCount}]...");
            return result;
        }
    }
}
