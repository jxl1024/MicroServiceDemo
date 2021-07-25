using MicroService.AggregateService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    /// <summary>
    /// 团队服务调用
    /// </summary>
    public interface ITeamServiceClient
    {
        /// <summary>
        /// 服务调用
        /// </summary>
        /// <returns></returns>
        Task<List<AggregateTeam>> GetTeams();
    }
}
