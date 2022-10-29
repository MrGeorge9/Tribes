namespace Eucyon_Tribes.Models.DTOs.LeaderboardDTOs
{
    public class BuildingScoreDTO
    {
        public string KingdomName { get; }
        public int[] BuildingScore { get; }

        public BuildingScoreDTO(string kingdomName, int[] buildingScore)
        {
            KingdomName = kingdomName;
            BuildingScore = buildingScore;
        }
    }
}
