using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;

namespace Eucyon_Tribes.Factories
{
    public interface IKingdomFactory
    {
        bool CreateKingdom(User user, String name, World world);
        string CreateKingdomWithCoordinates(KingdomCreateRequestDTO request);
    }
}