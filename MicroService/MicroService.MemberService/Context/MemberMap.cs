using MicroService.MemberService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MemberService.Context
{
    public class MemberMap : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            // 配置生成的表名
            builder.ToTable("Member");
            // 设置表注释
            builder.HasComment("团队成员表");
            // 配置主键
            builder.HasKey(p => p.ClusterID);
            // 设置主键自增
            builder.Property(p => p.ClusterID).ValueGeneratedOnAdd();

            builder.Property(p => p.ID).IsRequired();
        }
    }
}
