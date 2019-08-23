using EventManagement.Infrastructure.Data;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EventManagement.Identity
{
    /// <summary>
    /// User store for users in the Event Managemement database.
    /// </summary>
    public class DatabaseUserStore : IUserStore
    {
        private readonly EventsDbContext _dbContext;

        public DatabaseUserStore(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User FindByUsername(string username)
        {
            var user = _dbContext.Users.AsNoTracking()
                .Where(u => u.EmailAddress == username || u.Username == username).SingleOrDefault();

            if (user == null)
                return null;

            return new User
            {
                SubjectId = user.Id.ToString(),
                Username = user.Name
            };
        }

        public bool ValidateCredentials(string username, string password)
        {
            var hash = password.Sha256();
            return _dbContext.Users.Any(
                u => (u.EmailAddress == username || u.Username == username) && u.Password == hash);
        }

        public User FindByExternalProvider(string provider, string userId)
        {
            throw new System.NotImplementedException();
        }

        public User AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            throw new System.NotImplementedException();
        }
    }
}