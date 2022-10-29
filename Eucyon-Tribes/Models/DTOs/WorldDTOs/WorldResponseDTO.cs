namespace Eucyon_Tribes.Models.DTOs.WorldDTOs
{
    public class WorldResponseDTO
    {
        public int Id { get; }
        public string Name { get; }
        public int KingdomCount { get; }

        public WorldResponseDTO(int id, string name, int kingdomCount)
        {
            Id = id;
            Name = name;
            KingdomCount = kingdomCount;
        }
    }
}
