using System.ComponentModel.DataAnnotations;

namespace Harfien.Application.DTO.Authentication
{
    public class RegisterClientDto
    {
        [Required(ErrorMessage = "the name is required")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "the password is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "the password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "the password must be at least 6 characters long")]
        public string Password { get; set; } = null!;

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to terms")]
        public bool AgreeTerms { get; set; }
    }
}
