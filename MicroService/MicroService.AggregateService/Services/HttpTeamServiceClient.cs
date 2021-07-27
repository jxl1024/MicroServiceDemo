using MicroService.AggregateService.Models;
using MicroService.Core.HttpClientConsul;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    public class HttpTeamServiceClient : ITeamServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConsulHttpClient _consulHttpClient;

        public HttpTeamServiceClient(IConfiguration configuration, IConsulHttpClient consulHttpClient)
        {
            _configuration = configuration;
            _consulHttpClient = consulHttpClient;
        }

        public async Task<List<AggregateTeam>> GetTeams()
        {
            //// 获取团队服务名称
            string serviceName = _configuration.GetSection("TeamServiceName").Value;
            // 获取团队服务链接
            string serviceLink = _configuration.GetSection("TeamServiceLink").Value;
            List<AggregateTeam> teams= await _consulHttpClient.GetAsync<List<AggregateTeam>>("http",serviceName,serviceLink);
            return teams;
        }
    }
}
