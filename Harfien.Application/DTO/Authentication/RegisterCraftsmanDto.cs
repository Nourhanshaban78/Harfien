using System.ComponentModel.DataAnnotations;

namespace Harfien.Application.DTO.Authentication
{
    public class RegisterCraftsmanDto
    {
        [Required(ErrorMessage = "the name is required")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "the Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50, MinimumLength = 11, ErrorMessage = "the phonenumber must be at least 11 characters long")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "the email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "the email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "the password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "the password must be at least 6 characters long")]
        public string Password { get; set; } = null!;

        public int AreaId { get; set; }

        public int YearsOfExperience { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to terms")]
        public bool AgreeTerms { get; set; }
    }
}
