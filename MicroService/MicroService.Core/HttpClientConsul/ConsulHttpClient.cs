using MicroService.Core.Cluster;
using MicroService.Core.Registry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.HttpClientConsul
{
    /// <summary>
    /// consul httpclient扩展
    /// </summary>
    public class ConsulHttpClient  : IConsulHttpClient
    {
        private readonly IServiceDiscovery serviceDiscovery;
        private readonly ILoadBalance loadBalance;
        private readonly IHttpClientFactory httpClientFactory;
        public ConsulHttpClient(IServiceDiscovery serviceDiscovery,
                                    ILoadBalance loadBalance,
                                    IHttpClientFactory httpClientFactory)
        {
            this.serviceDiscovery = serviceDiscovery;
            this.loadBalance = loadBalance;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// param name="ServiceSchme">服务名称:(http/https)</param>
        /// <param name="ServiceName">服务名称</param>
        /// <param name="serviceLink">服务路径</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string Serviceshcme, string ServiceName, string serviceLink)
        {
            // 1、获取服务
            List<ServiceUrl> serviceUrls = await serviceDiscovery.Discovery(ServiceName);

            // 2、负载均衡服务
            ServiceUrl serviceUrl = loadBalance.Select(serviceUrls);

            // 3、建立请求
            HttpClient httpClient = httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(serviceUrl.Url + serviceLink);

            // 3.1json转换成对象
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new Exception($"{ServiceName}服务调用错误");
            }
        }
    }
}
