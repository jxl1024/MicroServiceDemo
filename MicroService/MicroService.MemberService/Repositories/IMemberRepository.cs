using MicroService.MemberService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Repositories
{
    /// <summary>
    /// 团队成员仓储接口
    /// </summary>
    public interface IMemberRepository
    {
        Task<Member> GetMemberById(Guid id);
        Task< IEnumerable<Member>> GetMembers(Guid teamId);
        Task<int> Create(Member member);
        Task<int> Update(Member member);
        Task<int> Delete(Member member);
        Task<bool> MemberExists(Guid id);
    }
}
