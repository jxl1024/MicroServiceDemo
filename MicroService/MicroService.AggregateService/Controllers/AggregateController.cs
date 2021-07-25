using MicroService.AggregateService.Models;
using MicroService.AggregateService.Services;
using MicroService.MemberService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        private readonly ITeamServiceClient _teamServiceClient;
        private readonly IMemeberServiceClient _memeberServiceClient;
        public AggregateController(ITeamServiceClient teamServiceClient, IMemeberServiceClient memeberServiceClient)
        {
            _teamServiceClient = teamServiceClient;
            _memeberServiceClient = memeberServiceClient;
        }

        // GET: api/Aggregate
        [HttpGet]
        public async Task<List<AggregateTeam>> Get()
        {
            // 1、查询团队
            List<AggregateTeam> teams = await _teamServiceClient.GetTeams();
            // 2、查询团队成员
            foreach (var team in teams)
            {

                List<Member> members = await _memeberServiceClient.GetMembersByTeamId(team.Id);

                team.Members = members;
            }

            return teams;
        }
    }
}
