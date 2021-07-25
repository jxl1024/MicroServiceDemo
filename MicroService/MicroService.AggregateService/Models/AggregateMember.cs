using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Models
{
    public class AggregateMember
    {
        /// <summary>
        /// 团队成员主键
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// 团队成员名
        /// </summary>
        public string FirstName { set; get; }
        /// <summaryhua
        /// 团队成员花名
        /// </summary>
        public string NickName { set; get; }

        /// <summary>
        /// 团队主键
        /// </summary>
        public Guid TeamId { set; get; }
    }
}
