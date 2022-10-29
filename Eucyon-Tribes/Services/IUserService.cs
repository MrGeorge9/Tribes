using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.UserModels;

namespace Eucyon_Tribes.Services
{
    public interface IUserService
    {
        string Login(UserLoginDto login);
        Dictionary<int, string> CreateUser(UserCreateDto user, string kingdomName, int worldId);
        List<UserResponseDto> ListAllUsers(int page, int itemCount);
        string DeleteUser(string name, string password);
        User UserInfo(string name);
        List<UserDetailDto> UsersInfoDetailedForAdmin(string adminPass);
        UserResponseDto ShowUser(int id);
        bool DestroyUser(int id, string password);
        bool EditUser(int id, string name, string password);
        bool UpdateUser(int id, UserCreateDto user);
        int StoreUsers(UsersInputDto users);
        string VerifyUserWithEmail(string token);
        string NewTokenGeneration(UserLoginDto login);
        string NewPasswordRequest(EmailForPasswordResetDto emailForPasswordResetDto);
        string NewPasswordVerification(string token);
        string NewPasswordGeneration(string token, NewPasswordDTO newPasswordDTO);
    }
}