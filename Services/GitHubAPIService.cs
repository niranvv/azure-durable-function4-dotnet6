using Learn.DurableFunction.Models;
using Learn.DurableFunction.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Learn.DurableFunction.ActivityFunctions
{
    public class GitHubAPIService : IGitHubAPIService
    {

        private readonly string _username;
        private readonly string _password;
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://api.github.com";

        public GitHubAPIService()
        {
            _username = Environment.GetEnvironmentVariable("GitHubUsername");
            _password = Environment.GetEnvironmentVariable("GitHubPassword");
        }
        public async Task<RepoViewCount> GetRepositoryViewCount(string repoName)
        {
            //log.LogInformation($"Fetch View Count for {repoName}. Starting...");

            var result = new RepoViewCount { RepoName = repoName };
            PrepareHttpClient();
            var url = $"{BaseUrl}/repos/{_username}/{repoName}/traffic/views";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            dynamic data = JObject.Parse(body);
            result.ViewCount = data.count;

            //log.LogInformation($"GOT View Count for {repoName} as [{result.ViewCount}]...");

            return result;
        }

        public async Task<List<string>> GetUserRepositoryList()
        {
            //log.LogInformation($"Fetch RepositoryList. Starting...");

            var result = new List<string>();
            PrepareHttpClient();
            var url = $"{BaseUrl}/users/{_username}/repos";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            List<JObject> jObjects = JsonConvert.DeserializeObject<List<JObject>>(body);
            foreach (var jObject in jObjects)
            {
                string repoName = jObject["name"].Value<string>();
                result.Add(repoName);
            }
            //log.LogInformation($"GOT RepositoryList total as [{result.Count}] repositories...");
            return result;
        }

        private void PrepareHttpClient()
        {
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("C# agent");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}")));
        }
    }
}
