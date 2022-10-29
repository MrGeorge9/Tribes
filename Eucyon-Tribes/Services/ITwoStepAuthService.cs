using Eucyon_Tribes.Models.DTOs.TwoStepAuthDTOs;

namespace Eucyon_Tribes.Services
{
    public interface ITwoStepAuthService
    {
        public AuthRegistrationDTO AuthRegistration(String username);

        public Boolean AuthLogin(String username, String inputCode);
    }
}
