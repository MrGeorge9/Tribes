namespace Eucyon_Tribes.Models.DTOs.BattleDTOs
{
    public class BattleCostDTO
    {
        public int food { get; }
        public int gold { get; }

        public BattleCostDTO(int food)
        {
            this.food = food;
            //this.gold = gold;
        }
    }
}
