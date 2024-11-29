namespace TaskApp.Models.DTOS.Request
{
    public class TaskRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
