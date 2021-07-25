using MicroService.TeamService.Models;
using MicroService.TeamService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.TeamService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await _teamService.GetTeams();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(Guid id)
        {
            var team = await _teamService.GetTeamById(id);

            if (team == null)
            {
                return NotFound();
            }
            return team;
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public async Task<int> PutTeam([FromBody]Team team)
        {
              return await  _teamService.Update(team);
        }

        // POST: api/Teams
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<int> PostTeam([FromBody]Team team)
        {
           return await _teamService.Create(team);
        }

        // DELETE: api/Teams/5
        [HttpDelete]
        public async Task<int> DeleteTeam(Guid id)
        {
            var team = await _teamService.GetTeamById(id);
            return await _teamService.Delete(team);
        }
    }
}
