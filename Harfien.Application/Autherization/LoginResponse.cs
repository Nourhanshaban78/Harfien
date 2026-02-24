using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Autherization
{
    public class LoginResponse
    {
     
            public bool Success { get; set; }
            public string? Token { get; set; }
            public string? Message { get; set; }

    }
}
