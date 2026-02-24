using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;

namespace Harfien.Domain.Interface_Repository.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetAdminAsync();
        Task<ApplicationUser> GetByIdAsync(string userid);


    }
}
