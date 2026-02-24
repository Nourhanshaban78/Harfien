using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;
using Harfien.Domain.Interface_Repository.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Harfien.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetAdminAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            return users.First();
        }

        public async Task<ApplicationUser> GetByIdAsync(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }

    }

}
