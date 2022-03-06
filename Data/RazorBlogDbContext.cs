using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BlogApp.Models;

namespace BlogApp.Data
{
    public class RazorBlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public RazorBlogDbContext(DbContextOptions<RazorBlogDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<BanTicket> BanTicket { get; set; }
        public DbSet<Community> Community { get; set; }
        public DbSet<ModeratorAssignment> CommunityModeration { get; set; }
        public DbSet<CommunitySubscription> CommunitySubscription { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ModerationHistory>()
                .HasOne(mh => mh.Community)
                .WithMany()
                .HasForeignKey(mh => mh.CommunityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(u => !u.IsDeleted);

            #region BanTicket 
            modelBuilder.Entity<BanTicket>()
                .HasOne(b => b.Moderator)
                .WithMany()
                .HasForeignKey(b => b.ModeratorId)
                .IsRequired();

            modelBuilder.Entity<BanTicket>()
                .HasOne(b => b.AppUser)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BanTicket>()
                .HasQueryFilter(cm => !cm.IsDeleted);

            modelBuilder.Entity<BanTicket>()
                .HasKey(b => new { b.UserId, b.ModeratorId });
            #endregion

            #region Community
            modelBuilder.Entity<Community>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorUserId)
                .IsRequired();

            modelBuilder.Entity<Community>()
                .HasQueryFilter(cm => !cm.IsDeleted);
            #endregion

            #region ModeratorAssignment
            modelBuilder.Entity<ModeratorAssignment>()
                .HasOne(c => c.Moderator)
                .WithMany()
                .HasForeignKey(c => c.ModeratorId)
                .IsRequired();

            modelBuilder.Entity<ModeratorAssignment>()
                .HasOne(c => c.Community)
                .WithMany()
                .HasForeignKey(c => c.CommunityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ModeratorAssignment>()
                .HasKey(cm => new { cm.CommunityId, cm.ModeratorId });

            modelBuilder.Entity<ModeratorAssignment>()
                .HasQueryFilter(cm => !cm.IsDeleted);
            #endregion

            #region CommunitySubscription
            modelBuilder.Entity<CommunitySubscription>()
                .HasOne(c => c.AppUser)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<CommunitySubscription>()
                .HasOne(c => c.Community)
                .WithMany()
                .HasForeignKey(c => c.CommunityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CommunitySubscription>()
                .HasKey(cs => new { cs.UserId, cs.CommunityId });
            #endregion
        }
    }
}
