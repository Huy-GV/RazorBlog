using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorBlog.Data.Constants;
using RazorBlog.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RazorBlog.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new RazorBlogDbContext(serviceProvider.GetRequiredService<DbContextOptions<RazorBlogDbContext>>());
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await EnsureRole(Roles.AdminRole, roleManager);
            await EnsureRole(Roles.ModeratorRole, roleManager);
            await EnsureAdminUser(userManager);
            await EnsureDefaultTopics(
                userManager.GetUsersInRoleAsync(Roles.AdminRole).Result.Single().Id,
                context);
        }

        private static async Task EnsureDefaultTopics(string adminId, RazorBlogDbContext context)
        {
            var seededTopicNames = new string[]
                {
                    "Technology", "Entertainment", "Health", "Education"
                };
            var topics = await context.Topic.Where(t => seededTopicNames.Contains(t.Name)).ToListAsync();

            if (topics.Count != 4)
            {
                context.Topic.RemoveRange(topics);
                await context.SaveChangesAsync();
                context.AddRange(new List<Topic>
            {
                 new Topic
                 {
                    CreationDate = DateTime.Now,
                    CreatorUserId = adminId,
                    Name = "Technology",
                    Description = "Lorem ipsum sed di em dema bent unari",
                 },
                 new Topic
                 {
                    CreationDate = DateTime.Now,
                    CreatorUserId = adminId,
                    Name = "Entertainment",
                    Description = "Lorem ipsum sed di em dema bent unari",
                 },
                 new Topic
                 {
                    CreationDate = DateTime.Now,
                    CreatorUserId = adminId,
                    Name = "Health",
                    Description = "Lorem ipsum sed di em dema bent unari",
                 },
                 new Topic
                 {
                    CreationDate = DateTime.Now,
                    CreatorUserId = adminId,
                    Name = "Education",
                    Description = "Lorem ipsum sed di em dema bent unari",
                 },
            });
            }

            await context.SaveChangesAsync();
        }

        private static async Task EnsureAdminUser(UserManager<ApplicationUser> userManager)
        {
            var (userName, password) = ("admin", "Admin123@@");
            var user = await userManager.FindByNameAsync(userName);

            if (user != null)
            {
                if (!await userManager.IsInRoleAsync(user, Roles.AdminRole))
                {
                    await userManager.AddToRoleAsync(user, Roles.AdminRole);
                }

                return;
            }

            user = new ApplicationUser
            {
                UserName = userName,
                EmailConfirmed = true,
                ProfileImageUri = "default.jpg",
                Description = "Lorem ipsum dolor sed temda met sedim ips dolor sed temda met sedim ips dolor sed temda met sedim ips"
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Roles.AdminRole);
        }

        private static async Task EnsureRole(string roleName, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}