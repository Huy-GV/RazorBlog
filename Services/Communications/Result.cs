namespace RazorBlog.Services.Communications
{
    public record Result
    {
        public bool Succeeded => Code == ServiceCode.Success && Message == string.Empty;
        public ServiceCode Code { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}