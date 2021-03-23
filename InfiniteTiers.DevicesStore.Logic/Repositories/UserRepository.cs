using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region private fields
        private readonly AuthDbContext _context;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public UserRepository(AuthDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        } 
        #endregion

        #region private Methods
        private IdentityRole GetRoleByName(string name)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == name);
            return role;
        }

        private IdentityRole GetRoleById(string id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            return role;
        }

        private IdentityUserRole<string> GetUserRoleByRole(string id)
        {
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.RoleId == id);
            return userRole;
        }

        private IdentityUserRole<string> GetUserRoleByUser(string id)
        {
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id);
            return userRole;
        }
        #endregion

        #region public methods
        public ApplicationUser GetUserByRole(string role)
        {
            var Role = GetRoleByName(role);
            if (Role == null)
            {
                return null;
            }
            var UserRole = GetUserRoleByRole(Role.Id);
            if (UserRole == null)
            {
                return null;

            }
            var User = GetUserById(UserRole.UserId);
            return User;
        }

        public ApplicationUser GetUserById(string id)
        {
            var user = _context.Users
                       .Include(u => u.Devices)
                       .FirstOrDefault(u => u.Id == id);
            return user;
        }

        public IdentityRole GetRoleByUser(string id)
        {
            var User = GetUserById(id);
            if (User == null)
            {
                return null;
            }
            var UserRole = GetUserRoleByUser(User.Id);
            if (UserRole == null)
            {
                return null;
            }
            var Role = GetRoleById(UserRole.RoleId);
            return Role;
        } 
        #endregion

    }
}
