using System.ComponentModel.DataAnnotations;

namespace Harfien.Application.DTO.Authentication
{
    public class loginDto
    {
        [Required(ErrorMessage = "the Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "the email is not valid")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "the password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "the password must be at least 6 characters long")]
        public string Password { get; set; } = null!;
    }
}
