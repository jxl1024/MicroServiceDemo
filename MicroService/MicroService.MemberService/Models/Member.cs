using MicroService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Models
{
    /// <summary>
    /// 团队成员模型
    /// </summary>
    public class Member :BaseModel
    {
        /// <summary>
        /// 团队成员名
        /// </summary>
        public string FirstName { set; get; }

        /// <summary>
        /// 团队成员花名
        /// </summary>
        public string NickName { set; get; }

        /// <summary>
        /// 团队主键
        /// </summary>
        public Guid TeamId { set; get; }
    }
}
