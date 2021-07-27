using MicroService.AggregateService.Models;
using MicroService.Core.Cluster;
using MicroService.Core.Registry;
using MicroService.MemberService.Models;
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
    public class HttpMemberServiceClient : IMemeberServiceClient
    {
        public readonly IServiceDiscovery _serviceDiscovery;
        public readonly ILoadBalance _loadBalance;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HttpMemberServiceClient(IServiceDiscovery serviceDiscovery,
                              ILoadBalance loadBalance,
                              IHttpClientFactory httpClientFactory,
                              IConfiguration configuration)
        {
            _serviceDiscovery = serviceDiscovery;
            _loadBalance = loadBalance;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        public async Task<List<Member>> GetMembersByTeamId(Guid teamId)
        {
            // 获取团队服务名称
            string serviceName = _configuration.GetSection("MemberServiceName").Value;
            // 获取团队服务链接
            string serviceLink =$"{_configuration.GetSection("MemberServiceLink").Value}?teamId={teamId}";
            // 1、获取服务
            List<ServiceUrl> serviceUrls = await _serviceDiscovery.Discovery(serviceName);

            // 2、负载均衡服务
            ServiceUrl serviceUrl = _loadBalance.Select(serviceUrls);

            // 3、建立请求
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(serviceUrl.Url + serviceLink);

            // 3.1json转换成对象
            List<Member> members = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();

                members = JsonConvert.DeserializeObject<List<Member>>(json);
            }
            return members;
        }
    }
}
