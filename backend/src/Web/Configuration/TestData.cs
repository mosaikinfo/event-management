using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Models;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace EventManagement.WebApp.Configuration
{
    public class TestData : ISeedData
    {
        public IList<User> Users => new[]
        {
            new User
            {
                Name = "Demo Admin",
                Username = "admin",
                EmailAddress = "event-admin@itsnotabug.de",
                Password = "admin".Sha256(),
                Role = UserRole.Admin
            }
        };
    }
}