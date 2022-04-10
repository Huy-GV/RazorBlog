namespace BlogApp.Services.Results
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
