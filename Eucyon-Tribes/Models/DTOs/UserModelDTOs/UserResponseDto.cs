namespace Eucyon_Tribes.Models.UserModels
{
    public class UserResponseDto
    {
        public int Id { get; }
        public string Username { get; }
        public DateTime Verified_at { get; }

        public UserResponseDto(int id, string userName, DateTime verifiedAt)
        {
            Id = id;
            Username = userName;
            Verified_at = verifiedAt;
        }
    }
}
