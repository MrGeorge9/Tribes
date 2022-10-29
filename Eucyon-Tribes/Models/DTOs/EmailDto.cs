namespace Eucyon_Tribes.Models.DTOs
{
    public class EmailDto
    {
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailDto(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
