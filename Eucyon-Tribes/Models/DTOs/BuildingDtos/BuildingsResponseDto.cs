namespace Eucyon_Tribes.Models.DTOs.BuildingDTOs
{
    public class BuildingsResponseDto
    {
        public int Id { get; }
        public int Kingdom { get; }
        public string Type { get; }
        public int Level { get; }
        public ProductionDto Production { get; }

        public BuildingsResponseDto()
        {
        }

        public BuildingsResponseDto(string type)
        {
            Type = type;
        }

        public BuildingsResponseDto(int id, int kingdom, string type, int level, ProductionDto production)
        {
            Id = id;
            Kingdom = kingdom;
            Type = type;
            Level = level;
            Production = production;
        }
    }
}
