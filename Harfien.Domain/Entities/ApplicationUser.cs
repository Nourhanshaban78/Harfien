using Harfien.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
 namespace Harfien.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Zone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public Wallet? Wallet { get; set; }
        public int? AreaId { get; set; }
        public Area? Area { get; set; }
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }
        public string? PasswordResetSession { get; set; }
        public DateTime? PasswordResetSessionExpiry { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<ChatMessage>? SentMessages { get; set; }

      


      
        

    }
}
