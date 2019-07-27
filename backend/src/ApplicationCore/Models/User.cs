using System.Collections.Generic;

namespace EventManagement.ApplicationCore.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool Enabled { get; set; } = true;

        public List<MasterQrCode> MasterQrCodes { get; set; }
    }
}