namespace RazorBlog.Services.Communications
{
    public record UploadedFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public System.DateTime UploadTime { get; set; }
    }
}