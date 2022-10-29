using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;

namespace Eucyon_Tribes.Services
{
    public interface ILeaderboardService
    {
        BuildingLeaderboardDTO GetBuildingLeaderboard(List<Kingdom> kingdoms);
        BuildingScoreDTO[] GetBuildingScoreDTOs(List<Kingdom> kingdoms);
        int[] CalculateBuildingScore(List<Building> buildings);
        SoldierLeaderboardDTO GetSoldierLeaderboard(List<Kingdom> kingdoms);
        SoldierScoreDTO[] GetSoldierScoreDTOs(List<Kingdom> kingdoms);
        int CalculateSoldierScore(List<Army> armies);
    }
}
