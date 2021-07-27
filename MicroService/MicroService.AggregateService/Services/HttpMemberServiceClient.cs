using MicroService.Core.HttpClientConsul;
using MicroService.MemberService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    public class HttpMemberServiceClient : IMemeberServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConsulHttpClient _consulHttpClient;

        public HttpMemberServiceClient(IConfiguration configuration, IConsulHttpClient consulHttpClient)
        {
            _configuration = configuration;
            _consulHttpClient = consulHttpClient;
        }


        public async Task<List<Member>> GetMembersByTeamId(Guid teamId)
        {
            // 获取成员服务名称
            string serviceName = _configuration.GetSection("MemberServiceName").Value;
            // 获取成员服务链接
            string serviceLink =$"{_configuration.GetSection("MemberServiceLink").Value}?teamId={teamId}";
            List<Member> members = await _consulHttpClient.GetAsync<List<Member>>("http", serviceName, serviceLink);
            return members;
        }
    }
}
