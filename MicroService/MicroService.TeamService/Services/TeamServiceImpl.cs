using MicroService.TeamService.Models;
using MicroService.TeamService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.TeamService.Services
{
    /// <summary>
    /// 团队服务实现
    /// </summary>
    public class TeamServiceImpl : ITeamService
    {
        public readonly ITeamRepository _teamRepository;

        public TeamServiceImpl(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<int> Create(Team team)
        {
            return await _teamRepository.Create(team);
        }

        public async Task<int> Delete(Team team)
        {
            return await _teamRepository.Delete(team);
        }

        public async Task<Team> GetTeamById(Guid id)
        {
            return await _teamRepository.GetTeamById(id);
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await _teamRepository.GetTeams();
        }

        public async Task<int> Update(Team team)
        {
            var entity = await _teamRepository.GetTeamById(team.ID);
            entity.Name = team.Name;
            return await _teamRepository.Update(entity);
        }

        public async Task<bool> TeamExists(Guid id)
        {
            return await _teamRepository.TeamExists(id);
        }
    }
}
