using MicroService.MemberService.Context;
using MicroService.MemberService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Repositories
{
    /// <summary>
    /// 成员仓储实现
    /// </summary>
    public class MemberRepository : IMemberRepository
    {
        public MemberDbContext _memberDbContext;
        public MemberRepository(MemberDbContext memberDbContext)
        {
            _memberDbContext = memberDbContext;
        }

        public async Task<int> Create(Member member)
        {
            await _memberDbContext.Members.AddAsync(member);
            return await _memberDbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(Member member)
        {
            _memberDbContext.Members.Remove(member);
            return await _memberDbContext.SaveChangesAsync();
        }

        public async Task<Member> GetMemberById(Guid id)
        {
            return await _memberDbContext.Members.FirstOrDefaultAsync(p=>p.ID == id);
        }

        public IEnumerable<Member> GetMembers()
        {
            return _memberDbContext.Members.ToList();
        }

        public async Task<int> Update(Member member)
        {
            _memberDbContext.Members.Update(member);
            return await _memberDbContext.SaveChangesAsync();
        }

        public async Task<bool> MemberExists(Guid id)
        {
            return await _memberDbContext.Members.AnyAsync(p => p.ID == id);
        }

        public async Task<IEnumerable<Member>> GetMembers(Guid teamId)
        {
            return await _memberDbContext.Members.Where(memeber => memeber.TeamId == teamId).ToListAsync();
        }
    }
}
