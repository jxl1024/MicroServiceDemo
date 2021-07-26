using Consul;
using MicroService.AggregateService.Models;
using MicroService.Core.Cluster;
using MicroService.Core.Registry;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    public class HttpTeamServiceClient : ITeamServiceClient
    {
        public readonly IServiceDiscovery _serviceDiscovery;
        public readonly ILoadBalance _loadBalance;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HttpTeamServiceClient(IServiceDiscovery serviceDiscovery,
                                    ILoadBalance loadBalance,
                                    IHttpClientFactory httpClientFactory,
                                    IConfiguration configuration)
        {
            _serviceDiscovery = serviceDiscovery;
            _loadBalance = loadBalance;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<AggregateTeam>> GetTeams()
        {
            // 获取团队服务名称
            string serviceName = _configuration.GetSection("TeamServiceName").Value;
            // 获取团队服务链接
            string serviceLink= _configuration.GetSection("TeamServiceLink").Value;
            // 1、获取服务
            List<ServiceUrl> serviceUrls = await _serviceDiscovery.Discovery(serviceName);

            // 2、负载均衡服务
            ServiceUrl serviceUrl = _loadBalance.Select(serviceUrls);

            // 3、建立请求
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(serviceUrl.Url + serviceLink);

            // 3.1json转换成对象
            List<AggregateTeam> teams = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();

                teams = JsonConvert.DeserializeObject<List<AggregateTeam>>(json);
            }

            return teams;
        }
    }
}
