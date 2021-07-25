using MicroService.TeamService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace MicroService.TeamService.Context
{
    public class TeamMap : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            // 配置生成的表名
            builder.ToTable("Team");
            // 设置表注释
            builder.HasComment("团队表");
            // 配置主键
            builder.HasKey(p => p.ClusterID);
            // 设置主键自增
            builder.Property(p => p.ClusterID).ValueGeneratedOnAdd();

            builder.Property(p => p.ID).IsRequired();
        }
    }
}
