using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;

namespace Eucyon_Tribes.Services
{
    public interface IKingdomService
    {
        Boolean AddKingdom(CreateKingdomDTO createKingdomDTO);
        List<Kingdom> GetKingdomsWorld(World world);
        KingdomsDTO[] GetKingdoms(int page, int itemCount);
        KingdomDTO GetKindom(int id);
        String GetError();
        bool WorldExists(int worldId);
        KingdomCreateResponseDTO AddKingdomWithLocation(KingdomCreateRequestDTO request);
        List<Kingdom> GetAllKingdoms();
        List<BattleResposeDto> GetBattles(int page, int itemCount);
    }
}

