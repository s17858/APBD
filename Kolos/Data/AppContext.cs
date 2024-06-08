namespace Kolos.Data;

using Microsoft.EntityFrameworkCore;
using Kolos.Models;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(c => c.IdClient);
            modelBuilder.Entity<Discount>().HasKey(d => d.IdDiscount);
            modelBuilder.Entity<Payment>().HasKey(p => p.IdPayment);
            modelBuilder.Entity<Sale>().HasKey(s => s.IdSale);
            modelBuilder.Entity<Subscription>().HasKey(s => s.IdSubscription);
            
            
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.IdClient);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.IdSubscription);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.IdClient);

            
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Subscription)
                .WithMany(s => s.Sales)
                .HasForeignKey(s => s.IdSubscription);
        }
    }
