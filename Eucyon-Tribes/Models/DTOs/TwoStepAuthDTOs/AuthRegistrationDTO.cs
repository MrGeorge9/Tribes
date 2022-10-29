namespace Eucyon_Tribes.Models.DTOs.TwoStepAuthDTOs
{
    public class AuthRegistrationDTO
    {
        public String ManualCode { get; }
        public String QR { get; }
        public String Description { get; }

        public AuthRegistrationDTO(string manualCode, string qR)
        {
            ManualCode = manualCode;
            QR = qR;
            Description = "Please download some 2 factor authentication application and either scan the QR code or insert the manual one";
        }
    }
}
