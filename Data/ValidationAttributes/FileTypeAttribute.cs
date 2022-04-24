using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace RazorBlog.Data.ValidationAttributes
{
    public class FileTypeAttribute : ValidationAttribute
    {
        private readonly string[] allowedFileTypes;

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                return allowedFileTypes.Contains(Path.GetExtension(file.FileName).TrimStart('.'));
            }

            return false;
        }

        public FileTypeAttribute(params string[] allowedTypes)
        {
            if (allowedTypes
                .Where(x => string.IsNullOrWhiteSpace(x) || x == string.Empty)
                .Any())
            {
                throw new System.ArgumentException("File types must not be null or empty");
            }

            allowedFileTypes = allowedTypes
                .Select(x => x.TrimStart('.', ' '))
                .ToArray();
        }
    }
}