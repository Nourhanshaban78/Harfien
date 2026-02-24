using Harfien.Application.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Profile_Craftman
{
    public class CraftsmanProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public int YearsOfExperience { get; set; }
        public double Rating { get; set; }
        public bool IsVerified { get; set; }

        public List<ServiceDto> Services { get; set; }
        public List<AvailabilityDto> Availabilities { get; set; }
        public  List<ReviewsDto> Reviews { get; set; }  

        public int CompletedOrdersCount { get; set; }
    }
}
