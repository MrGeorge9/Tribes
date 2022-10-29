namespace Eucyon_Tribes.Models.UserModels
{
    public class UsersInputDto
    {
        public List<UserCreateDto> users { get; set; }

        public UsersInputDto()
        {
            users = new List<UserCreateDto>();
        }
    }
}
