namespace EventManagement.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
