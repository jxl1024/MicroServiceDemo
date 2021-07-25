using MicroService.AggregateService.Models;
using MicroService.MemberService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    /// <summary>
    /// 成员服务调用
    /// </summary>
    public interface IMemeberServiceClient
    {
        Task<List<Member>> GetMembersByTeamId(Guid teamId);
    }
}
