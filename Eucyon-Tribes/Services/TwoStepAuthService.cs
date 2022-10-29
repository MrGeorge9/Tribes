using Eucyon_Tribes.Models.DTOs.TwoStepAuthDTOs;
using Google.Authenticator;
using System.Text;

namespace Eucyon_Tribes.Services
{
    public class TwoStepAuthService : ITwoStepAuthService
    {
        private readonly IConfiguration _config;
        public TwoStepAuthService(IConfiguration configuration)
        { 
            _config = configuration;
        }
        public bool AuthLogin(String username,String inputCode)
        {
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
            String secret = $"{Environment.GetEnvironmentVariable("TwoStepSecret")}-{username}";
            return twoFactorAuthenticator.ValidateTwoFactorPIN(secret, inputCode);
        }

        public AuthRegistrationDTO AuthRegistration(String username)
        {
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
            String secret = $"{Environment.GetEnvironmentVariable("TwoStepSecret")}-{username}";
            var setupInfo = twoFactorAuthenticator.GenerateSetupCode("Eucyon tribes",username , Encoding.ASCII.GetBytes(secret),300);
            String manualCode = setupInfo.ManualEntryKey;
            String QR= setupInfo.QrCodeSetupImageUrl;
            return new AuthRegistrationDTO(manualCode, QR);
        }
    }
}
