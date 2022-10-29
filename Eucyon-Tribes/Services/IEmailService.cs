using Eucyon_Tribes.Models.DTOs;

namespace Eucyon_Tribes.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
