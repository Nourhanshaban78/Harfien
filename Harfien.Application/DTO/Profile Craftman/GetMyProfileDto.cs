using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Profile_Craftman
{
    public class GetMyProfileDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsVerified { get; set; }

        public List<ServiceDto> Services { get; set; }
    }
}
