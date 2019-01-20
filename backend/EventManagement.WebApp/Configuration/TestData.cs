using EventManagement.DataAccess;
using EventManagement.DataAccess.Models;
using EventManagement.WebApp.Models;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace EventManagement.WebApp.Configuration
{
    public class TestData : IEventsDbInitialData
    {
        public IList<User> Users => new[]
        {
            new User
            {
                Name = "Demo Admin",
                EmailAddress = "event-admin@itsnotabug.de",
                Password = "Start123$".Sha256(),
                Role = UserRoles.Admin
            }
        };
    }
}
