using EventManagement.DataAccess;
using EventManagement.Identity;
using IdentityServer4.Models;
using System.Collections.Generic;
using User = EventManagement.DataAccess.Models.User;

namespace EventManagement.WebApp.Configuration
{
    public class TestData : IEventsDbInitialData
    {
        public IList<User> Users => new[]
        {
            new User
            {
                Name = "Demo Admin",
                Username = "admin",
                EmailAddress = "event-admin@itsnotabug.de",
                Password = "admin".Sha256(),
                Role = UserRoles.Admin
            }
        };
    }
}
