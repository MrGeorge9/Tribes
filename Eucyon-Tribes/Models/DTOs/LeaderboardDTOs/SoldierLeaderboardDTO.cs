namespace Eucyon_Tribes.Models.DTOs.LeaderboardDTOs
{
    public class SoldierLeaderboardDTO
    {
        public List<SoldierScoreDTO> Leaderboard { get; }

        public SoldierLeaderboardDTO(List<SoldierScoreDTO> leaderboard)
        {
            Leaderboard = leaderboard;
        }
    }
}
