using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository (AuthDbContext context)
        {
            _context = context;
        }

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

        public ApplicationUser GetUserById(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public ApplicationUser GetUserByRole(string role)
        {
            var Role = GetRoleByName(role);
            var UserRole = GetUserRoleByRole(Role.Id);
            var User = GetUserById(UserRole.UserId);
            return User;
        }

        public IdentityRole GetRoleByUser(string id)
        {
            var User = GetUserById(id);
            var UserRole = GetUserRoleByUser(User.Id);
            var Role = GetRoleById(UserRole.RoleId);
            return Role;
        }

    }
}
