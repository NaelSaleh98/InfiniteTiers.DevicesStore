using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface IUserRepository
    {
        public ApplicationUser GetUserByRole(string role);
        public ApplicationUser GetUserById(string id);

        public IdentityRole GetRoleByUser(string id);
    }
}
