namespace Eucyon_Tribes.Models.DTOs
{
    public class NewPasswordDTO
    {
        public string NewPassword { get; }

        public NewPasswordDTO(string newPassword)
        {
            NewPassword = newPassword;
        }
    }
}
