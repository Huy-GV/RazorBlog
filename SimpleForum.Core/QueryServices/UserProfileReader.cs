using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Core.Communication;
using SimpleForum.Core.Data;
using SimpleForum.Core.Data.Dtos;

namespace SimpleForum.Core.QueryServices;

internal class UserProfileReader : IUserProfileReader
{
    private readonly IDbContextFactory<SimpleForumDbContext> _dbContextFactory;
    private readonly IAggregateImageUriResolver _aggregateImageUriResolver;
    private readonly IDefaultProfileImageProvider _defaultProfileImageProvider;
    public UserProfileReader(
        IDbContextFactory<SimpleForumDbContext> dbContextFactory,
        IAggregateImageUriResolver aggregateImageUriResolver,
        IDefaultProfileImageProvider defaultProfileImageProvider)
    {
        _dbContextFactory = dbContextFactory;
        _aggregateImageUriResolver = aggregateImageUriResolver;
        _defaultProfileImageProvider = defaultProfileImageProvider;
    }

    public async Task<(ServiceResultCode, PersonalProfileDto?)> GetUserProfileAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return (ServiceResultCode.InvalidArguments, null);
        }

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            return (ServiceResultCode.InvalidArguments, null);
        }

        var threads = dbContext.Thread
            .Include(thread => thread.AuthorUser)
            .AsNoTracking()
            .Where(thread => thread.AuthorUser.UserName == userName)
            .ToList();

        var threadsGroupedByYear = threads
            .GroupBy(x => x.CreationTime.Year)
            .OrderByDescending(g => g.Key)
            .ToDictionary(
        group => (uint)group.Key,
                group => (IReadOnlyCollection<ThreadHistoryEntryDto>)group.Select(x => new ThreadHistoryEntryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ViewCount = x.ViewCount,
                    CreationTime = x.CreationTime,
                    LastUpdateTime = x.LastUpdateTime
                })
                .ToList()
            );

        var profileDto = new PersonalProfileDto
        {
            UserName = userName,
            ThreadCount = (uint)threads.Count,
            ProfileImageUri = await _aggregateImageUriResolver.ResolveImageUriAsync(user.ProfileImageUri)
                              ?? _defaultProfileImageProvider.GetDefaultProfileImageUri(),
            ThreadsGroupedByYear = (System.Collections.Generic.IReadOnlyDictionary<uint, System.Collections.Generic.IReadOnlyCollection<ThreadHistoryEntryDto>>)threadsGroupedByYear,
            Description = string.IsNullOrEmpty(user.Description)
                ? "None"
                : user.Description,
            CommentCount = (uint)dbContext.Comment
                .Include(c => c.AuthorUser)
                .Where(c => c.AuthorUser.UserName == userName)
                .ToList()
                .Count,
            ThreadCountCurrentYear = (uint)threads
                .Where(thread => thread.AuthorUser.UserName == userName &&
                               thread.CreationTime.Year == DateTime.UtcNow.Year)
                .ToList()
                .Count,
            ViewCountCurrentYear = (uint)threads
                .Where(thread => thread.AuthorUser.UserName == userName &&
                               thread.CreationTime.Year == DateTime.UtcNow.Year)
                .Sum(thread => thread.ViewCount),
            RegistrationDate = user.RegistrationDate == null
                    ? "a long time ago"
                    : user.RegistrationDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        };

        return (ServiceResultCode.Success, profileDto);
    }
}
