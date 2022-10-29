namespace Eucyon_Tribes.Models.DTOs
{
    public class EmailForPasswordResetDto
    {
        public string Email { get; }

        public EmailForPasswordResetDto(string email)
        {
            Email = email;
        }
    }
}
