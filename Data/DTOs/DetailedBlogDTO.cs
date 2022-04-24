using RazorBlog.Models;
using System;
using System.Collections.Generic;

namespace RazorBlog.Data.DTOs
{
    public class DetailedBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePicture { get; set; }
        public string AuthorDescription { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsHidden { get; set; }
        public ICollection<CommentDto> CommentDtos { get; set; }

        public static DetailedBlogDto From(Blog blog)
        {
            List<CommentDto> commentDtos = new();
            foreach (var comment in blog.Comments)
            {
                commentDtos.Add(new CommentDto()
                {
                    Id = comment.Id,
                    Date = comment.Date,
                    AuthorName = comment.Author,
                    IsHidden = comment.IsHidden,
                    AuthorProfilePicturePath = comment.AppUser?.ProfileImageUri ?? "default.jpg"
                });
            }

            var description = $"Joined on {blog.AppUser.RegistrationDate}";
            if (!string.IsNullOrEmpty(blog.AppUser.Description))
            {
                description = blog.AppUser.Description;
            }
            return new DetailedBlogDto()
            {
                Id = blog.Id,
                Title = blog.Title,
                AuthorName = blog.Author,
                AuthorDescription = description,
                AuthorProfilePicture = blog.AppUser.ProfileImageUri,
                Description = blog.Introduction,
                IsHidden = blog.IsHidden,
                Date = blog.Date,
                CommentDtos = commentDtos
            };
        }
    }
}