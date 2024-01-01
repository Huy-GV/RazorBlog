﻿namespace RazorBlog.Data.Dtos;

public record CurrentUserInfo
{
    public bool AllowedToModifyOrDeletePost { get; init; }
    public bool AllowedToHidePost { get; init; }
    public bool IsBanned { get; init; }
    public bool IsAuthenticated { get; init; }
    public required string UserName { get; init; }
}