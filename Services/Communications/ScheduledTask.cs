using System;

namespace RazorBlog.Services.Communications
{
    public record ScheduledTask
    {
        public DateTime ExecutionTime { get; set; }
        public string ScheduledAction { get; set; }
        public DateTime ScheduleTime { get; set; }
        public string UserId { get; set; }
    }
}