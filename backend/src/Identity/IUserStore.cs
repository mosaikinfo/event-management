using System.Collections.Generic;
using System.Security.Claims;

namespace EventManagement.Identity
{
    public interface IUserStore
    {
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        bool ValidateCredentials(string username, string password);

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        User FindByUsername(string username);

        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        User FindByExternalProvider(string provider, string userId);

        /// <summary>
        /// Automatically provisions a user.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        User AutoProvisionUser(string provider, string userId, List<Claim> claims);
    }
}
