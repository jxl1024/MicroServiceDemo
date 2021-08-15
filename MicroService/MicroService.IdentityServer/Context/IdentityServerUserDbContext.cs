using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroService.IdentityServer.Context
{
    public class IdentityServerUserDbContext : IdentityDbContext<IdentityUser>
    {
        public IdentityServerUserDbContext(DbContextOptions<IdentityServerUserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
