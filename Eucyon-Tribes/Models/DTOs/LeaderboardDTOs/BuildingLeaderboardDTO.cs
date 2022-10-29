namespace Eucyon_Tribes.Models.DTOs.LeaderboardDTOs
{
    public class BuildingLeaderboardDTO
    {
        public List<BuildingScoreDTO> Leaderboard { get; }

        public BuildingLeaderboardDTO(List<BuildingScoreDTO> leaderboard)
        {
            Leaderboard = leaderboard;
        }
    }
}
