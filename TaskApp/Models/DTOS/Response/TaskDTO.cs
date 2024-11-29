namespace TaskApp.Models.DTOS.Response
{
    public class TaskDTO
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
