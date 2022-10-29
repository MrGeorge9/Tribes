namespace Eucyon_Tribes.Models.DTOs.KingdomDTOs
{
    public class KingdomCreateResponseDTO
    {
        public int StatusCode { get; }
        public string Message { get; }
        public bool Error { get; }

        public KingdomCreateResponseDTO(int statusCode, string message, bool error)
        {
            StatusCode = statusCode;
            Message = message;
            Error = error;
        }
    }
}
