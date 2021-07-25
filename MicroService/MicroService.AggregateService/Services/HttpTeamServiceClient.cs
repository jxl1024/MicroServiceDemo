using MicroService.AggregateService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    public class HttpTeamServiceClient : ITeamServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpTeamServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<AggregateTeam>> GetTeams()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:5000/api/teams");

            string json = await response.Content.ReadAsStringAsync();
            List<AggregateTeam> teams = JsonConvert.DeserializeObject<List<AggregateTeam>>(json);
            return teams;
        }
    }
}
