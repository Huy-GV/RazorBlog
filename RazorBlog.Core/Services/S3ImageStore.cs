﻿using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;
using RazorBlog.Core.Utils;
using RazorBlog.Core.Communication;
using RazorBlog.Core.Data.Constants;
using RazorBlog.Core.Options;

namespace RazorBlog.Core.Services;

public class S3ImageStore : IImageStore
{
    private readonly ILogger<S3ImageStore> _logger;
    private readonly IAmazonS3 _awsS3Client;
    private readonly AwsOptions _awsS3Options;
    private readonly IDefaultProfileImageProvider _defaultProfileImageProvider;

    private const string AwsS3UrlFormat = "https://{0}.s3.{1}.amazonaws.com/{2}";

    public S3ImageStore(
        ILogger<S3ImageStore> logger,
        IAmazonS3 awsS3Client,
        IOptions<AwsOptions> awsS3Options,
        IDefaultProfileImageProvider defaultProfileImageProvider)
    {
        _logger = logger;
        _awsS3Client = awsS3Client;
        _awsS3Options = awsS3Options.Value;
        _defaultProfileImageProvider = defaultProfileImageProvider;
    }

    public async Task<ServiceResultCode> DeleteImage(string uri)
    {
        if (!AwsUtils.TryConvertToS3Uri(uri, out var s3Uri))
        {
            _logger.LogError("Failed to convert image URI '{uri}' to S3 URI", uri);
            return ServiceResultCode.InvalidState;
        }

        var request = new DeleteObjectRequest
        {
            Key = s3Uri.Key,
            BucketName = _awsS3Options.DataBucket,
        };

        _logger.LogInformation("Deleting image with key '{key}'", request.Key);

        try
        {
            var deleteObjectResponse = await _awsS3Client.DeleteObjectAsync(request);

            if (deleteObjectResponse.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return ServiceResultCode.Success;
            }

            _logger.LogError(
                "Failed to delete image, HTTP code: {code}",
                deleteObjectResponse.HttpStatusCode);
            return ServiceResultCode.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred: {ex}", ex);
            return ServiceResultCode.Error;
        }
    }

    public async Task<string> GetDefaultProfileImageUriAsync()
    {
        return await _defaultProfileImageProvider.GetDefaultProfileImageUriAsync();
    }

    private static string BuildFileName(string originalName, string type)
    {
        return string.Join(
            "_",
            DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            type,
            originalName
                .Trim('.', '_', '@', ' ', '#', '/', '\\', '!', '^', '&', '*')
                .Replace(".", string.Empty)
                .Replace("_", string.Empty)
                .Replace("@", string.Empty)
                .Replace(" ", string.Empty)
                .Replace("#", string.Empty)
                .Replace("/", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("!", string.Empty)
                .Replace("^", string.Empty)
                .Replace("&", string.Empty)
                .Replace("*", string.Empty)
            );
    }

    private string BuildImageUrl(string objectKey)
    {
        var region = _awsS3Client.Config.RegionEndpoint.SystemName;
        return string.Format(AwsS3UrlFormat, _awsS3Options.DataBucket, region, objectKey);
    }

    public async Task<(ServiceResultCode, string?)> UploadBlogCoverImageAsync(IFormFile imageFile)
    {
        var tag = new Tag
        {
            Key = nameof(ImageType),
            Value = ImageType.BlogCover.ToString(),
        };

        var putBlogCoverImageRequest = new PutObjectRequest
        {
            Key = BuildFileName(imageFile.FileName, ImageType.BlogCover.ToString()),
            BucketName = _awsS3Options.DataBucket,
            InputStream = imageFile.OpenReadStream(),
            TagSet = [tag],
        };

        _logger.LogInformation(
            "Uploading blog cover image '{fileName}' to bucket '{bucketName}'",
            putBlogCoverImageRequest.Key,
            putBlogCoverImageRequest.BucketName);

        try
        {
            var putObjectResponse = await _awsS3Client.PutObjectAsync(putBlogCoverImageRequest);
            if (putObjectResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.LogError(
                    "Failed to upload blog cover image, HTTP code: {code}",
                    putObjectResponse.HttpStatusCode);
                return (ServiceResultCode.Error, null);
            }

            var imageUrl = BuildImageUrl(putBlogCoverImageRequest.Key);

            return (ServiceResultCode.Success, imageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred: {ex}", ex);
            return (ServiceResultCode.Error, null);
        }
    }

    public async Task<(ServiceResultCode, string?)> UploadProfileImageAsync(IFormFile imageFile)
    {
        var tag = new Tag
        {
            Key = nameof(ImageType),
            Value = ImageType.ProfileImage.ToString(),
        };

        var putProfileImageRequest = new PutObjectRequest
        {
            Key = BuildFileName(imageFile.FileName, ImageType.ProfileImage.ToString()),
            BucketName = _awsS3Options.DataBucket,
            InputStream = imageFile.OpenReadStream(),
            TagSet = [tag],
        };

        _logger.LogInformation(
            "Uploading profile image '{fileName}' to bucket '{bucketName}'",
            putProfileImageRequest.Key,
            putProfileImageRequest.BucketName);

        try
        {
            var putObjectResponse = await _awsS3Client.PutObjectAsync(putProfileImageRequest);
            if (putObjectResponse.HttpStatusCode != HttpStatusCode.OK)
            {

                _logger.LogError(
                    "Failed to upload profile image, HTTP code: {code}",
                    putObjectResponse.HttpStatusCode);
                return (ServiceResultCode.Error, null);
            }

            var imageUrl = BuildImageUrl(putProfileImageRequest.Key);

            return (ServiceResultCode.Success, imageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred: {ex}", ex);
            return (ServiceResultCode.Error, null);
        }
    }
}
