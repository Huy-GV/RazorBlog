﻿namespace RazorBlog.Data.Dtos;

public class UserProfileDto
{
    public required string UserName { get; init; }
    public required string RegistrationDate { get; init; }
}