using MicroService.MemberService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Context
{
    /// <summary>
    /// 数据上下文，继承自DbContext
    /// </summary>
    public class MemberDbContext : DbContext
    {
        /// <summary>
        /// 通过构造函数传参
        /// </summary>
        /// <param name="options"></param>
        public MemberDbContext(DbContextOptions<MemberDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberMap());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Member> Members { get; set; }
    }
}
