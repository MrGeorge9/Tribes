namespace Eucyon_Tribes.Models.DTOs
{
    public class ResponseDTO
    {
        public String Status { get; }

        public ResponseDTO (string status)
        {
            Status = status;
        }

    }
}
