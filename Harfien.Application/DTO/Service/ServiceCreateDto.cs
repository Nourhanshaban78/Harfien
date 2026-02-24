using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;

namespace Harfien.Application.DTO.Service
{
    public class ServiceCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "CraftsmanProfileId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CraftsmanId must be greater than 0")]
        public int CraftsmanId { get; set; }

        [Required(ErrorMessage = "ServiceCategoryId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ServiceCategoryId must be greater than 0")]
        public int ServiceCategoryId { get; set; }
        
    }
}
