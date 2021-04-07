using HSM.WebApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HSM.WebApp.Data
{
    public class HsmDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<MemberAccount> MemberAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionPassthrough> TransactionPassthrough { get; set; }
        public DbSet<Charges> Charges { get; set; }

        public HsmDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}