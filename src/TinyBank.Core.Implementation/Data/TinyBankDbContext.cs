using Microsoft.EntityFrameworkCore;

using TinyBank.Core.Model;

namespace TinyBank.Core.Implementation.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class TinyBankDbContext : DbContext
    {
        public TinyBankDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(
                builder => {
                    builder.ToTable("Customer", "model");

                    builder
                        .HasIndex(c => c.VatNumber)
                        .IsUnique();

                    builder.OwnsOne(c => c.AuditInfo);
                });

            modelBuilder.Entity<Account>(
                builder => {
                    builder.ToTable("Account", "model");

                    builder.OwnsOne(c => c.AuditInfo);
                });

            modelBuilder.Entity<Card>(
                builder => {
                    builder.ToTable("Card", "model");

                    builder
                        .HasIndex(c => c.CardNumber)
                        .IsUnique();
                });
        }
    }
}
