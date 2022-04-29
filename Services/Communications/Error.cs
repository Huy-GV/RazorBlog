namespace RazorBlog.Services.Communications
{
    /// <summary>
    /// Represents default error object.
    /// </summary>
    /// <param name="Message"></param>
    public record Error(string Message = "");
}