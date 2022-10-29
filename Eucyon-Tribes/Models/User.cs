using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Eucyon_Tribes.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string VerificationToken { get; set; } = null!;
        public DateTime VerificationTokenExpiresAt { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ForgottenPasswordToken { get; set; } = null!;
        public DateTime ForgottenPasswordTokenExpiresAt { get; set; }
        public DateTime ForgottenPasswordTokenVerifiedAt { get; set; }

        public Kingdom Kingdom { get; set; } = null!;

        public User()
        {
            VerifiedAt = DateTime.MinValue;
            ForgottenPasswordTokenVerifiedAt = DateTime.MinValue;
            VerificationTokenExpiresAt = DateTime.Now.AddHours(1);            
            CreatedDate = DateTime.Now;
        }

        public void setVerificationExpiration(int hours)
        {
            this.VerificationTokenExpiresAt = DateTime.Now.AddHours(hours);
        }
    }
}