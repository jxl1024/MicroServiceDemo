using MicroService.MemberService.Models;
using MicroService.MemberService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService memberService;

        public MembersController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        /// <summary>
        /// 查询所有成员信息
        /// </summary>
        /// <param name="teamId">?teamId参数结尾方式</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Member>> GetMembers(Guid teamId)
        {
                return await memberService.GetMembers(teamId);
        }


        // GET: api/Members/5
        [HttpGet("{id}")]
        public async  Task<Member> GetMember(Guid id)
        {
            return await memberService.GetMemberById(id);
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public async Task<int> PutMember([FromBody]Member member)
        {
              return await  memberService.Update(member);
        }

        // POST: api/Members
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<int> PostMember([FromBody]Member member)
        {
            return await memberService.Create(member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<int> DeleteMember(Guid id)
        {
            var member = await memberService.GetMemberById(id);
            return await memberService.Delete(member);
        }
    }
}
