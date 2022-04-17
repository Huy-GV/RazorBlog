using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using RazorBlog.Models;

namespace RazorBlog.Data
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
        public DbSet<Topic> Community { get; set; }
        public DbSet<ModeratorAssignment> CommunityModeration { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            #endregion BanTicket

            #region Topic

            modelBuilder.Entity<Topic>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorUserId)
                .IsRequired();

            modelBuilder.Entity<Topic>()
                .HasQueryFilter(cm => !cm.IsDeleted);

            #endregion Topic

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

            #endregion ModeratorAssignment
        }
    }
}