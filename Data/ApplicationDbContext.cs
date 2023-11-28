using BankingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*builder.Entity<User>()
       .HasOne(a => a.Balance)
       .WithOne(a => a.User)
       .HasForeignKey<Balance>(a => a.UserId)
       .HasPrincipalKey<User>(a => a.Id);*/
        }
        public DbSet<BankingSystem.Models.Balance>? Balance { get; set; }
		public DbSet<BankingSystem.Models.Transaction>? Transaction { get; set; }
	}
}