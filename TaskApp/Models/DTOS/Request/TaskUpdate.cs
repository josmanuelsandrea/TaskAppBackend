namespace TaskApp.Models.DTOS.Request
{
    public class TaskUpdate
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
