namespace Eucyon_Tribes.Models.DTOs.KingdomDTOs
{
    public class CreateKingdomDTO
    {
        public int UserId { get; }
        public int WorldId { get; }
        public string Name { get; }

        public CreateKingdomDTO(int userId, int worldId, string name)
        {
            UserId = userId;
            WorldId = worldId;
            Name = name;
        }
    }
}
