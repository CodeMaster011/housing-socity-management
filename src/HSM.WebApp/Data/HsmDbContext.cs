using HSM.WebApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HSM.WebApp.Data
{
    public class HsmDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Unit> Units { get; set; }

        public HsmDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}