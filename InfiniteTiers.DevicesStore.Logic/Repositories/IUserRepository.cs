using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get first user by role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>first user match this role.</returns>
        public ApplicationUser GetUserByRole(string role);

        /// <summary>
        /// Get specific user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user</returns>
        public ApplicationUser GetUserById(string id);

        /// <summary>
        /// Get role of specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Role</returns>
        public IdentityRole GetRoleByUser(string id);
    }
}
