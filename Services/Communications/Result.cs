namespace RazorBlog.Services.Communications
{
    public record Result
    {
        public bool Succeeded => Code == ServiceCode.Success && ErrorMessage == string.Empty;
        public ServiceCode Code { get; init; }
        public string ErrorMessage { get; init; } = string.Empty;
    }
}