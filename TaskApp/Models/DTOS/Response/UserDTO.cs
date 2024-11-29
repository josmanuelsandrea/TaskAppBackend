namespace TaskApp.Models.DTOS.Response
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
