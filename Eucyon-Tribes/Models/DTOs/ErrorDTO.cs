namespace Eucyon_Tribes.Models.DTOs
{
    public class ErrorDTO
    {
        public string Error { get; }

        public ErrorDTO (string error)
        {
            Error = error;
        }
    }
}

