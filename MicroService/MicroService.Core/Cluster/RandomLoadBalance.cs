﻿using MicroService.Core.Registry;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Core.Cluster
{
    /// <summary>
    /// 随机负载均衡
    /// 1、还可以实现加权轮询
    /// </summary>
    public class RandomLoadBalance : AbstractLoadBalance
    {
        private readonly Random random = new Random();

        public override ServiceUrl DoSelect(List<ServiceUrl> serviceUrls)
        {
            // 1、获取随机数
            var index = random.Next(serviceUrls.Count);

            // 2、选择一个服务进行连接
            return serviceUrls[index];
        }
    }
}
