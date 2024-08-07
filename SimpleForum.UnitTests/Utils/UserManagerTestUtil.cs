﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SimpleForum.Core.Models;

namespace SimpleForum.UnitTests.Utils;

internal class UserManagerTestUtil
{
    internal static Mock<UserManager<ApplicationUser>> CreateUserManagerMock(
        IUserStore<ApplicationUser>? userStore = null)
    {
        return new Mock<UserManager<ApplicationUser>>(
            userStore ?? new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
    }
}
