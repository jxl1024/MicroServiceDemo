using MicroService.TeamService.Context;
using MicroService.TeamService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.TeamService.Repositories
{
    /// <summary>
    /// 团队仓储实现
    /// </summary>
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamDbContext _teamContext;

        /// <summary>
        /// 构造函数实现注入
        /// </summary>
        /// <param name="teamContext"></param>
        public TeamRepository(TeamDbContext teamContext)
        {
            this._teamContext = teamContext;
        }
        public async Task<int> Create(Team team)
        {
            await _teamContext.Teams.AddAsync(team);
            return await _teamContext.SaveChangesAsync();
        }

        public async Task<int> Delete(Team team)
        {
             _teamContext.Teams.Remove(team);
            return await _teamContext.SaveChangesAsync();
        }

        public async  Task<Team> GetTeamById(Guid id)
        {
            return await _teamContext.Teams.FirstOrDefaultAsync(p=>p.ID == id);
        }

        public async  Task<IEnumerable<Team>> GetTeams()
        {
            return await _teamContext.Teams.ToListAsync();
        }

        public async Task<int> Update(Team team)
        {
            _teamContext.Teams.Update(team);
            return await _teamContext.SaveChangesAsync();
        }
        public async  Task<bool> TeamExists(Guid id)
        {
            return await _teamContext.Teams.AnyAsync(p => p.ID == id);
        }
    }
}
