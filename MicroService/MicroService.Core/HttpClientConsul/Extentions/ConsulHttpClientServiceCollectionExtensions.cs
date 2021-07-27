using MicroService.Core.Cluster;
using MicroService.Core.Registry.Extentions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Core.HttpClientConsul.Extentions
{
    /// <summary>
    /// HttpClientFactory conusl下的扩展
    /// </summary>
    public static class ConsulHttpClientServiceCollectionExtensions
    {
        /// <summary>
        /// 添加consul
        /// </summary>
        /// <typeparam name="ConsulHttpClient"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientConsul(this IServiceCollection services)
        {
            // 1、注册consul
            services.AddConsulDiscovery();

            // 2、注册服务负载均衡
            services.AddSingleton<ILoadBalance, RandomLoadBalance>();

            // 3、注册httpclient
            services.AddSingleton<IConsulHttpClient,ConsulHttpClient>();

            return services;
        }
    }
}
