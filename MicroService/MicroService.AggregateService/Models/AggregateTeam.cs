using MicroService.MemberService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Models
{
    public class AggregateTeam
    {
        /// <summary>
        /// 团队主键
        /// </summary>
        public Guid Id { set; get; }

        /// <summary>
        /// 团队名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 团队成员
        /// </summary>
        public List<Member> Members { set; get; }
    }
}
