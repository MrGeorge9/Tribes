namespace Eucyon_Tribes.Models.DTOs.BuildingDTOs
{
    public class BuildingResponseDto
    {
        public int Id { get; }
        public int Kingdom { get; }
        public string Type { get; }
        public int Level { get; }
        public ProductionDto Production { get; }
        public LevelingCostDto Leveling_Cost { get; }

        public BuildingResponseDto(string type)
        {
            Type = type;
        }

        public BuildingResponseDto(int id, int kingdom, string type, int level, ProductionDto production, LevelingCostDto leveling_Cost)
        {
            Id = id;
            Kingdom = kingdom;
            Type = type;
            Level = level;
            Production = production;
            Leveling_Cost = leveling_Cost;
        }
    }
}
