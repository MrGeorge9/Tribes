namespace Eucyon_Tribes.Models.UserModels
{
    public class UserCreateDto
    {
        public string Name { get; }
        public string Password { get; }
        public string Email { get; }

        public UserCreateDto(string name, string password, string email)
        {
            Name = name;
            Password = password;
            Email = email;
        }
    }
}