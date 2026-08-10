using ChatBot_Indexing.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ChatBot_Indexing.Clients
{
    public class FootballDataClient
    {
        private const string BaseUrl = "https://api.football-data.org/v4/";

        private readonly HttpClient _httpClient;

        public FootballDataClient(HttpClient httpClient, IOptions<AppOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.FootballDataApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetChampionsLeagueMatchesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("competitions/CL/matches");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetChampionsLeagueTeamsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("competitions/CL/teams");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
