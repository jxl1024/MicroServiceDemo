using MicroService.TeamService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.TeamService.Services
{
    /// <summary>
    /// 团队服务接口
    /// </summary>
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeams();
        Task<Team> GetTeamById(Guid id);
        Task<int> Create(Team team);
        Task<int> Update(Team team);
        Task<int> Delete(Team team);
        Task<bool> TeamExists(Guid id);
    }
}
