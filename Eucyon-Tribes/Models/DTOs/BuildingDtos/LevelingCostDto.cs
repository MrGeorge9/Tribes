namespace Eucyon_Tribes.Models.DTOs.BuildingDTOs
{
    public class LevelingCostDto
    {
        public int Gold { get; }
        public int Food { get; }

        public LevelingCostDto()
        {
        }

        public LevelingCostDto(int gold, int food)
        {
            Gold = gold;
            Food = food;
        }
    }
}
