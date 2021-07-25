using MicroService.TeamService.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroService.TeamService.Context
{
    /// <summary>
    /// 数据上下文，继承自DbContext
    /// </summary>
    public class TeamDbContext :DbContext
    {
        /// <summary>
        /// 通过构造函数传参
        /// </summary>
        /// <param name="options"></param>
        public TeamDbContext(DbContextOptions<TeamDbContext> options):base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamMap());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Team> Teams { get; set; }
    }
}
