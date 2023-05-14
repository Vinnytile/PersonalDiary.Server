using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using SharedData.Models.User;
using System;

namespace DataAccess.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserIdentity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserIdentity>()
                .HasMany(e => e.Notes)
                .WithOne(e => e.UserIdentity)
                .HasForeignKey(e => e.UserIdentityFID)
                .IsRequired();

            modelBuilder.Entity<UserIdentity>()
                .HasOne(e => e.UserProfile)
                .WithOne(e => e.UserIdentity)
                .HasForeignKey<UserProfile>(e => e.UserIdentityFID)
                .IsRequired();

            modelBuilder.Entity<UserIdentity>()
                .HasMany(e => e.Subscribers)
                .WithOne(e => e.Subscriber)
                .HasForeignKey(e => e.SubscriberFID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserIdentity>()
                .HasMany(e => e.Observables)
                .WithOne(e => e.Observable)
                .HasForeignKey(e => e.ObservableFID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>()
                .HasIndex(s => new
                { 
                    s.SubscriberFID,
                    s.ObservableFID
                })
                .IsUnique();
        }
    }
}
