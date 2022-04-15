namespace BlogApp.Data.DTOs
{
    public record BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string AuthorName { get; set; }
        public uint CommentCount { get; set; }
        public string TopicName { get; set; }
    }
}