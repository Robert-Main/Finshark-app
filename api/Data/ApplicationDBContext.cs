using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Portfolio>()
                .HasKey(p => new { p.AppUserId, p.StockId });

            builder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(p => p.Stock)
                .WithMany(s => s.Portfolios)
                .HasForeignKey(p => p.StockId);

            builder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId);

            builder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StockId);

            List<IdentityRole> roles = new()
                {
                    new() { Id = "c6ca4556-85fc-4ed5-b870-e87714a7d044", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "d35d3f42-6db1-4565-9fd3-b0e563ed245f" },
                    new() { Id = "9952465f-eecf-47a0-a86b-511ddb2bf4cd", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "5fc0d6e5-5320-4c20-9767-9318a80d0ea5" }
                };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}