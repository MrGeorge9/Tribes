using Eucyon_Tribes.Models;

namespace Eucyon_Tribes.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user, string purpose);
        int ValidateToken(string token);
    }
}
