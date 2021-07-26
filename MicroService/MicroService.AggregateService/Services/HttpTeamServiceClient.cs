using Consul;
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
            // 1、创建consul客户端连接
            var consulClient = new ConsulClient(configuration =>
            {
                //1.1 建立客户端和服务端连接
                configuration.Address = new Uri("http://127.0.0.1:8500");
            });
            // 2、consul查询服务,根据具体的服务名称查询
            var queryResult = await consulClient.Catalog.Service("teamservice");

            // 3、将服务进行拼接
            var list = new List<string>();
            foreach (var service in queryResult.Response)
            {
                list.Add(service.ServiceAddress + ":" + service.ServicePort );
            }

            HttpClient httpClient = _httpClientFactory.CreateClient();
            //HttpResponseMessage response = await httpClient.GetAsync("http://localhost:5000/api/teams");
            // 这里为了测试，取第一个地址，真实项目中应该根据负载均衡去获取地址
            HttpResponseMessage response = await httpClient.GetAsync($"{list[0]}/api/teams");

            string json = await response.Content.ReadAsStringAsync();
            List<AggregateTeam> teams = JsonConvert.DeserializeObject<List<AggregateTeam>>(json);
            return teams;
        }
    }
}
