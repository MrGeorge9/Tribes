using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;

namespace Eucyon_Tribes.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        public BuildingLeaderboardDTO GetBuildingLeaderboard(List<Kingdom> kingdoms)
        {
            if (kingdoms == null || kingdoms.Count == 0)
            {
                return new BuildingLeaderboardDTO(new List<BuildingScoreDTO>());
            }
            BuildingScoreDTO[] buildingScoreDTOs = GetBuildingScoreDTOs(kingdoms);
            BuildingLeaderboardDTO leaderboardDTO = new BuildingLeaderboardDTO(buildingScoreDTOs.OrderByDescending(b => b.BuildingScore[0]).ThenByDescending(b => b.BuildingScore[1]).ToList());
            return leaderboardDTO;
        }

        public BuildingScoreDTO[] GetBuildingScoreDTOs(List<Kingdom> kingdoms)
        {
            BuildingScoreDTO[] result = new BuildingScoreDTO[kingdoms.Count];
            for (int i = 0; i < kingdoms.Count; i++)
            {
                result[i] = new BuildingScoreDTO(kingdoms[i].Name, CalculateBuildingScore(kingdoms[i].Buildings));
            }
            return result;
        }

        public int[] CalculateBuildingScore(List<Building> buildings)
        {
            if (buildings == null || buildings.Count == 0)
            {
                return new int[] {0, 0};
            }
            int[] result = new int[2];
            result[0] = buildings.Sum(b => b.Level);
            result[1] = buildings.Max(b => b.Level);
            return result;
        }

        public SoldierLeaderboardDTO GetSoldierLeaderboard(List<Kingdom> kingdoms)
        {
            if (kingdoms == null || kingdoms.Count == 0)
            {
                return new SoldierLeaderboardDTO(new List<SoldierScoreDTO>());
            }
            SoldierScoreDTO[] soldierScoreDTOs = GetSoldierScoreDTOs(kingdoms);
            SoldierLeaderboardDTO leaderboardDTO = new SoldierLeaderboardDTO(soldierScoreDTOs.OrderByDescending(s => s.SoldierScore).ToList());
            return leaderboardDTO;
        }

        public SoldierScoreDTO[] GetSoldierScoreDTOs(List<Kingdom> kingdoms)
        {
            SoldierScoreDTO[] result = new SoldierScoreDTO[kingdoms.Count];
            for (int i = 0; i < kingdoms.Count; i++)
            {
                result[i] = new SoldierScoreDTO(kingdoms[i].Name, CalculateSoldierScore(kingdoms[i].Armies));
            }
            return result;
        }

        public int CalculateSoldierScore(List<Army> armies)
        {
            if (armies == null || armies.Count == 0)
            {
                return 0;
            }
            int result = 0;
            foreach (Army army in armies)
            {
                result += army.Soldiers.Sum(s => s.Attack);
            }
            return result;
        }
    }
}
