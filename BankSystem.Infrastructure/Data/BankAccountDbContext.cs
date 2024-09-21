using BankSystem.Domain.Authentication;
using BankSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace BankSystem.Infrastructure.Data
{
    public class BankAccountDbContext(DbContextOptions<BankAccountDbContext> options) 
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
