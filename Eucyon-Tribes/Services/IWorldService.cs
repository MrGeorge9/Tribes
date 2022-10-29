using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.WorldDTOs;

namespace Eucyon_Tribes.Services
{
    public interface IWorldService
    {
        WorldResponseDTO[] GetWorldsWithKingdoms();
        bool CreateWorld(string name);
        bool StoreWorld(StoreWorldDTO newWorldDTO);
        WorldDetailDTO GetWorldDetails(int id);
        bool EditWorld(int id, string name);
        bool UpdateWorld(int id, UpdateWorldDTO newWorldDTO);
        bool DestroyWorld(int id);
    }
}
