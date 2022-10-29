namespace Eucyon_Tribes.Models.DTOs.LeaderboardDTOs
{
    public class SoldierScoreDTO
    {
        public string KingdomName { get; }
        public int SoldierScore { get; }

        public SoldierScoreDTO(string kingdomName, int soldierScore)
        {
            KingdomName = kingdomName;
            SoldierScore = soldierScore;
        }
    }
}
