﻿using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Core.Options;

public class AwsS3Options
{
    public const string Name = "S3";

    [Required]
    public required string BucketName { get; init; }
}
