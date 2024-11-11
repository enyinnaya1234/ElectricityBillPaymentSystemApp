using Microsoft.EntityFrameworkCore;
using ElectricityBillPaymentSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ElectricityBillPaymentSystem.Infrastructure.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
    
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);

            builder.Entity<Bill>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
        }
    }
}
