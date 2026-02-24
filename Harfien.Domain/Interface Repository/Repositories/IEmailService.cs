using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Interface_Repository.Repositories
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
