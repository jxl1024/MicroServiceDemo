using MicroService.MemberService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Services
{
    /// <summary>
    /// 团队服务接口
    /// </summary>
    public interface IMemberService
    {
        //IEnumerable<Member> GetMembers();
        Task< Member> GetMemberById(Guid id);
        Task<int> Create(Member member);
        Task<int> Update(Member member);
        Task<int> Delete(Member member);
        Task<bool> MemberExists(Guid id);
        Task<IEnumerable<Member>> GetMembers(Guid teamId);
    }
}
