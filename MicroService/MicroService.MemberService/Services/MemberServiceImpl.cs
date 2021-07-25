using MicroService.MemberService.Models;
using MicroService.MemberService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Services
{
    /// <summary>
    /// 团队服务实现
    /// </summary>
    public class MemberServiceImpl : IMemberService
    {
        public readonly IMemberRepository memberRepository;

        public MemberServiceImpl(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }

        public async Task<int> Create(Member member)
        {
            return await memberRepository.Create(member);
        }

        public async Task<int> Delete(Member member)
        {
            return await memberRepository.Delete(member);
        }

        public async  Task<Member> GetMemberById(Guid id)
        {
            return await memberRepository.GetMemberById(id);
        }

        //public IEnumerable<Member> GetMembers()
        //{
        //    return memberRepository.GetMembers();
        //}

        public async Task<int> Update(Member member)
        {
            var entity = await memberRepository.GetMemberById(member.ID);
            entity.FirstName = member.FirstName;
            entity.NickName = member.NickName;
            entity.UpdateUser = "admin";
            entity.UpdateTime = DateTime.Now;
            return await memberRepository.Update(entity);
        }

        public async Task<bool> MemberExists(Guid id)
        {
            return await memberRepository.MemberExists(id);
        }

        public async Task<IEnumerable<Member>> GetMembers(Guid teamId)
        {
            return await memberRepository.GetMembers(teamId);
        }
    }
}
