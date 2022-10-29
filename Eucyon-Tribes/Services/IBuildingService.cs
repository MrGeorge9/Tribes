using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using Eucyon_Tribes.Models;

namespace Eucyon_Tribes.Services
{
    public interface IBuildingService
    {
        void SaveBuilding(Building building);
        Building GetBuildingById(int id);
        List<BuildingsResponseDto> GetAllBuldingsOfKingdom(int userId);
        Resource SetResourceType(int userId, string buldingType);
        BuildingResponseDto GetBuildingDetails(int id, int userId);
        BuildingCreationResponseDto DeleteBuildingById(int id, int userId);
        BuildingCreationResponseDto StoreNewBulding(BuildingRequestDto buildingRequestDto, int userId);
        Building CreateRightBuilding(int id);
        bool KingdomHasResourcesForBuildingCreation(Kingdom kingdom, Building building);
        BuildingCreationResponseDto UpgradeBuilding(int id, int userId);
    }
}